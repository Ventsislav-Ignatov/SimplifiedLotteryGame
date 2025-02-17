using System.ComponentModel.DataAnnotations;

namespace SimplifiedLotteryGame.Data.Models
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Balance { get; set; }

        public decimal? Profit { get; set; }

        public List<Ticket> Tickets { get; set; } = [];

        public List<PlayerPrize> PlayerPrizes { get; set; } = [];
    }
}
