using BGT_Web_Account.Helper;
using BGT_Web_Account.Models.Entity;
using BGT_Web_Account.Models.Request;

namespace BGT_Web_Account.Services.Account
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();
        Task<AccountModel> GetAsync(int id);
        Task<int> GetIdByMailAsync(string email);

        Task<ResultWrapper<AccountModel>> AddAsync(AccountModel account);
        Task DeleteAsync(int id);
        Task UpdateAsync(AccountModel account);

        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserExistsAsync(string username);
        Task<ResultWrapper<string>> ValidateLoginAsync(LoginRequest loginRequest);
    }
}
