using System.ComponentModel.DataAnnotations;

namespace SimplifiedLotteryGame.Data.Models
{
    public class Prize
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal PrizeValue { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<PlayerPrize> PlayerPrizes { get; set; } = new();
    }
}
