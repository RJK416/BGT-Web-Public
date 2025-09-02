using BGT_Web_Account.Models.Entity;

namespace BGT_Web_Account.Interfaces
{
    public interface IAccountRepository
    {
        Task<IEnumerable<AccountModel>> GetAllAsync();

        Task<AccountModel> GetAsync(int id);

        Task<int> GetIdByMail(string email);

        Task AddAsync(AccountModel model);

        Task UpdateAsync(AccountModel model);

        Task DeleteAsync(int id);

        Task SaveChangesAsync();

        Task<bool> userEmailExistsAsync(string userMail);
        Task<bool> userNameExistsAsync(string userName);

        Task<AccountModel> getUserByUsernameAsync(string username);
        Task<bool> userExistsAsync(int id);

    }
}
