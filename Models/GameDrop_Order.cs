using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Order
    {
        [Key]
        [Required]
        public int OrderId { get; set; }
        public string? OrderDate { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderAddress { get; set; }
        public string? OrderPhone { get; set; }
        public string? OrderEmail { get; set; }
    }
}
