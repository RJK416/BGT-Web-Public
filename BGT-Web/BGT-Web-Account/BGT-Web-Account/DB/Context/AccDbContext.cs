using BGT_Web_Account.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BGT_Web_Account.DB.Context
{
    public class AccDbContext : DbContext
    {
        public AccDbContext ( DbContextOptions<AccDbContext> options ) : base(options) {}

        public DbSet<AccountModel> Accounts { get; set; }
    }
}
