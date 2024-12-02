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
        public string? PaymentType { get; set; }
        public int? UserAddressId { get; set; }
        public string? UserId { get; set; }
    }
}
