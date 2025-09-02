using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Models.Entity;
using BGT_Web_Account.Services.Account;
using BGT_Web_Account.Services.OTP;
using Microsoft.AspNetCore.Mvc;
using BGT_Web_Account.Services.Authentication;
using BGT_Web_Account.Models.Request;

namespace BGT_Web_Account.Controllers
{
    [ApiController]
    [Route("Account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly OTPSender _OtpSender;
        private readonly OTPVerifier _OtpVerifier;

        public AccountController(IAccountService accountService, OTPSender OtpSender, OTPVerifier OtpVerifier)
        {
             _accountService = accountService;
            _OtpSender = OtpSender;
            _OtpVerifier = OtpVerifier;
        }


        [HttpGet("Hello")]
        public IActionResult Hello()
        {
            return Ok("Hello Developer !");
        }

        [HttpPost("Create1")]
        public async Task<IActionResult> CreateAccount(string userName, string userMail, string hashedPassword)
        {
            AccountModel NewAccount = new AccountModel()
            {
                userName = userName,
                userEmail = userMail,
                passwordHash = hashedPassword,
                isActive = true
            };

            var result = await _accountService.AddAsync(NewAccount);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    status = 409,
                    error = result.Message
                });
            }

            return Ok(new
            {
                status = 200,
                message = result.Message,
                data = result.Result
            });

        }

        [HttpPost("OtpSender")]
        public async Task<IActionResult> SendOtp_CreateAccount([FromBody] AccountCreateRequest accountCreateRequest)
        {
            PendingAccountModel PendingAccount = new PendingAccountModel()
            {
                userName = accountCreateRequest.Username,
                userEmail = accountCreateRequest.Mail,
                passwordHash = accountCreateRequest.Password,
            };

            var result = await _OtpSender.SendOtp(PendingAccount);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status409Conflict, new
                {
                    status = 409,
                    error = result.Message
                });
            }

            return Ok(new
            {
                status = 200,
                message = result.Message,
                data = result.Result
            });
        }

        [HttpPost("OtpVerifier")]
        public async Task<IActionResult> VerifyOtp_CreateAccount([FromBody] OtpRequest otpRequest)
        {
            var result = await _OtpVerifier.VerifyOtp(otpRequest.Otp, otpRequest.UserName);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    status = 400,
                    error = result.Message
                });
            }

            return Ok(new
            {
                status = 200,
                message = result.Message
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var result = await _accountService.ValidateLoginAsync(loginRequest);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = 401,
                    error = result.Message
                });
            }

            return Ok(new
            {
                status = 200,
                message = result.Message,
                data = result.Result
            });
        }

        [HttpPost("PasswordReset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest passwordReset)
        {
            var result = await _OtpSender.ResetPasswordOtpSender(passwordReset);

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new
                {
                    status = 401,
                    error = result.Message
                });
            }

            return Ok(new
            {
                status = 200,
                message = result.Message,
            });
        }
    }
}
