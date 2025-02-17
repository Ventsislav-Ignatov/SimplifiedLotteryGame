using Microsoft.EntityFrameworkCore;
using SimplifiedLotteryGame.Data.Models;

namespace SimplifiedLotteryGame.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Prize> Prizes { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<PlayerPrize> PlayerPrizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Apply entity configurations
            modelBuilder.ApplyConfiguration(new PlayerPrizeEntityConfiguration());

            // ✅ Apply cascade delete behavior globally
            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                         .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }
        }
    }
}
