using BCrypt.Net;
using BGT_Web_Account.Helper;
using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Models.Entity;
using BGT_Web_Account.Models.Request;
using BGT_Web_Account.Services.Authentication;
using BGT_Web_Account.Services.OTP;
using System.Security.Cryptography;
using System.Text;

namespace BGT_Web_Account.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AccountService(IAccountRepository accountRepository, JwtTokenGenerator jwtTokenGenerator)
        {
             _accountRepository = accountRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<ResultWrapper<AccountModel>> AddAsync(AccountModel account)
        {

            bool EmailExists = await _accountRepository.userEmailExistsAsync(account.userEmail);

            if (EmailExists) 
            {
                return ResultWrapper<AccountModel>.Fail(null, "This mail is already taken");
            }

            bool UserNameExists = await _accountRepository.userNameExistsAsync(account.userName);

            if (UserNameExists)
            {
                return ResultWrapper<AccountModel>.Fail(null, "This Username is alraedy taken");
            }


            account.passwordHash = BCrypt.Net.BCrypt.HashPassword(account.passwordHash);

            await _accountRepository.AddAsync(account);
            await _accountRepository.SaveChangesAsync();

            return ResultWrapper<AccountModel>.Ok(account, "Account created Sucesfuly");
        }

        public async Task<ResultWrapper<string>> ValidateLoginAsync(LoginRequest loginRequest)
        {
            var user = await _accountRepository.getUserByUsernameAsync(loginRequest.Username!);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.passwordHash))
            {
                return ResultWrapper<string>.Fail(null,"Incorrect Username or Password");
            }

            var token = _jwtTokenGenerator.GenerateToken
            (
                 userId: user.Id.ToString(),
                 username: user.userName,
                 mail: user.userEmail
            );

            return ResultWrapper<string>.Ok(token, "C");
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AccountModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AccountModel> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetIdByMailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AccountModel account)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsAsync(string username)
        {
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
