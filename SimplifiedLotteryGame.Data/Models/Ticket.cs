using System.ComponentModel.DataAnnotations;

namespace SimplifiedLotteryGame.Data.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Guid TicketNumber { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public int PlayerId { get; set; }

        public Player Player { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public List<PlayerPrize> PlayerPrizes { get; set; } = new();
    }
}
