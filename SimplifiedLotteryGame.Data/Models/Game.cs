using System.ComponentModel.DataAnnotations;

namespace SimplifiedLotteryGame.Data.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public decimal? Revenue { get; set; }

        public List<Ticket> Tickets { get; set; } = [];

        public List<Prize> Prizes { get; set; } = [];
    }
}
