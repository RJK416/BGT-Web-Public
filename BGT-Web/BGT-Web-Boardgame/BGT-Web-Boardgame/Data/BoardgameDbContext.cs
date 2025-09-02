using BGT_Web_Boardgame.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace BGT_Web_Boardgame.Data
{
    public class BoardgameDbContext : DbContext
    {
        public BoardgameDbContext(DbContextOptions<BoardgameDbContext> options ) 
            : base (options)
        {
             
        }

        public DbSet<BoardgameModel> Boardgames { get; set; }
        public DbSet<BoardgameOwnerModel> BoardgameOwners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BoardgameModel>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(200);
                b.HasIndex(x => x.Name).IsUnique();

                b.Property(x => x.AddTime)
                 .HasColumnType("timestamp with time zone")
                 .HasDefaultValueSql("now() at time zone 'utc'");

                b.HasMany(x => x.Owners)
                 .WithOne(o => o.Boardgame)
                 .HasForeignKey(o => o.BoardgameId)
                 .OnDelete(DeleteBehavior.Cascade);
            });


            var e = modelBuilder.Entity<BoardgameOwnerModel>();

            e.HasOne(b => b.Boardgame).WithMany()
             .HasForeignKey(b => b.BoardgameId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BoardgameOwnerModel>()
                .HasIndex(o => new { o.AccountId, o.BoardgameId })
                .IsUnique();

        }
    }
}
