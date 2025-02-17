using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SimplifiedLotteryGame.Data.Models
{
    public class PlayerPrize
    {
        public int PlayerId { get; set; }

        public Player Player { get; set; }

        public int PrizeId { get; set; }

        public Prize Prize { get; set; }

        public int TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public DateTime WinDate { get; set; } = DateTime.UtcNow;
    }

    public class PlayerPrizeEntityConfiguration : IEntityTypeConfiguration<PlayerPrize>
    {
        public void Configure(EntityTypeBuilder<PlayerPrize> builder)
        {
            builder.ToTable("PlayerPrize");

            builder.HasKey(pp => new { pp.PlayerId, pp.PrizeId, pp.TicketId });
        }
    }
}
