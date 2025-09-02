using BGT_Web_Account.DB.Context;
using BGT_Web_Account.Interfaces;
using BGT_Web_Account.Models.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BGT_Web_Account.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccDbContext _context;
        private readonly DbSet<AccountModel> _dbSet;

        public AccountRepository(AccDbContext context)
        {
             _context = context;
            _dbSet = context.Accounts;
        }
        public async Task AddAsync(AccountModel account)
        {
            await _dbSet.AddAsync(account);
        }

        public async Task DeleteAsync(int id)
        {
            var ModelExists = await _dbSet.FindAsync(id);

            if( ModelExists != null )
            {
                _dbSet.Remove(ModelExists);
            }
        }

        public async Task<IEnumerable<AccountModel>> GetAllAsync()
        {
             return await _dbSet.ToListAsync();
        }

        public async Task<AccountModel> GetAsync(int id)
        {
            var accountExists = await _dbSet.FindAsync(id);
            
            if (accountExists == null)
            {
                throw new KeyNotFoundException("No account found");
            }

            return accountExists;
        }

        public async Task<int> GetIdByMail(string email)
        {
            var accountExists = await _dbSet.FirstOrDefaultAsync(a => a.userEmail == email);

            if (accountExists == null)
            {
                throw new KeyNotFoundException("No account found");
            }

            return accountExists.Id;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AccountModel model)
        {
            _dbSet.Update(model);
            await Task.CompletedTask;
        }

        public async Task<AccountModel> getUserByUsernameAsync(string username)
        {

            var user = await _dbSet.FirstOrDefaultAsync(a => a.userName == username);

            if (user == null)
            {
                throw new KeyNotFoundException("User Not Found");
            }

            return user;
        }

        public async Task<bool> userEmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(a => a.userEmail == email);
        }

        public async Task<bool> userNameExistsAsync(string userName)
        {
            return await _dbSet.AnyAsync(a => a.userName == userName);
        }

        public  async Task<bool> userExistsAsync(int id)
        {
             var user = await  _dbSet.AnyAsync(a => a.Id == id);

            if (!user)
            {
                return false;
            }

            return user;

        }
    }
}
