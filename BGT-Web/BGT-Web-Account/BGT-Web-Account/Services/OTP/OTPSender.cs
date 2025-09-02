using BGT_Web_Account.Helper;
using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Models.Entity;
using BGT_Web_Account.Models.Request;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;
using System.Security.Principal;

namespace BGT_Web_Account.Services.OTP
{
    public class OTPSender
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailSender _emailSender;

        public OTPSender(IMemoryCache memoryCache, IAccountRepository accountRepository,IEmailSender emailSender)
        {
            _memoryCache = memoryCache;
            _accountRepository = accountRepository;   
            _emailSender = emailSender;
        }

        public async Task<ResultWrapper<string>> SendOtp(PendingAccountModel pendingAccount)
        {
            if ( await _accountRepository.userEmailExistsAsync(pendingAccount.userEmail) )
            {
                return ResultWrapper<string>.Fail(null, "This mail - " + pendingAccount.userEmail + " is already taken");
            }

            if ( await _accountRepository.userNameExistsAsync(pendingAccount.userName) )
            {
                return ResultWrapper<string>.Fail(null, "this userName - " + pendingAccount.userName + " is already taken");
            }

            pendingAccount.otp = RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString();
            pendingAccount.expiresAt = DateTime.UtcNow.AddMinutes(4);

            _memoryCache.Set($"otp_{pendingAccount.userName}",pendingAccount,TimeSpan.FromMinutes(4));

            await _emailSender.SendEmailAsync(pendingAccount.userEmail, 
                $"Board Games Tracker Verification" +
                "", $"Hello {pendingAccount.userName},\n\nWe are excited to have you here ! Lets have fun together\nThis is your OTP is: {pendingAccount.otp}\n\nBest Regards,\nBGT Team"
                );

            return ResultWrapper<string>.Ok(null, "Otp sent successfully");
        }


        public async Task<ResultWrapper<string>> ResetPasswordOtpSender(PasswordResetRequest passwordReset)
        {
            var user = await _accountRepository.getUserByUsernameAsync(passwordReset.Username!);

            if (user == null)
            {
                return ResultWrapper<string>.Fail(null, "User not found");
            }

            var resetAccount = new PendingAccountModel
            {
                userEmail = user.userEmail,
                userName = user.userName,
                passwordHash = user.passwordHash,
                otp = RandomNumberGenerator.GetInt32(100_000, 1_000_000).ToString(),
                expiresAt = DateTime.UtcNow.AddMinutes(4)
            };

            _memoryCache.Set($"otp_{resetAccount.userName}", resetAccount, TimeSpan.FromMinutes(4));

            await _emailSender.SendEmailAsync(resetAccount.userEmail,
                    $"Password reset request " +
                    "", $"Hello {resetAccount.userName},\n\nThis is your password reset mail ! \n\nIf you havent made this request please contact us ! \nThis is your OTP for passwod reset: {resetAccount.otp}\n\nBest Regards,\nBGT Team"
                    );

            return ResultWrapper<string>.Ok(null, "Otp sent successfully");
        }

    }
}
