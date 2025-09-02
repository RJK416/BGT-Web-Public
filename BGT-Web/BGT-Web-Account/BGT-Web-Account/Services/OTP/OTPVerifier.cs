using BGT_Web_Account.Helper;
using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Models.Entity;
using BGT_Web_Account.Services.Account;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;

namespace BGT_Web_Account.Services.OTP
{
    public class OTPVerifier
    {
        private readonly IMemoryCache _memorycache;
        private readonly IAccountService _accountService;

        public OTPVerifier(IMemoryCache memoryCache, IAccountService accountService)
        {
          _memorycache = memoryCache;
            _accountService = accountService;
        }

        public async Task<ResultWrapper<string>> VerifyOtp(string otpEntered, string username)
        {
            if (!_memorycache.TryGetValue($"otp_{username}", out var obj) || obj is not PendingAccountModel pendAcc)
            {
                return ResultWrapper<string>.Fail(null, "No Account found");
            }

            if (pendAcc.expiresAt < DateTime.UtcNow)
            {
                return ResultWrapper<string>.Fail(null, "OTP time has expired, please resend it");
            }

            if(pendAcc.otp != otpEntered)
            {
                return ResultWrapper<string>.Fail(null, "Incorrect Otp");
            }

            var result = await _accountService.AddAsync
                                            (
                                                 new AccountModel() 
                                                 {
                                                     userName = pendAcc.userName,
                                                     userEmail = pendAcc.userEmail,
                                                     passwordHash = pendAcc.passwordHash,
                                                 }
                                            );

            if (!result.Success)
            {
                return ResultWrapper<string>.Fail(null, result.Message);
            }

            _memorycache.Remove($"otp_{username}");
            return ResultWrapper<string>.Ok(null,result.Message);
        }
    }
}
