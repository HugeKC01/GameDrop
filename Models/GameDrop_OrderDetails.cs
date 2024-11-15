using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_OrderDetails
    {
        [Key]
        [Required]
        public int OrderDetailsId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
