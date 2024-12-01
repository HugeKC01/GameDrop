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
        [Display(Name = "Product")]
        public string? OrderProductName { get; set; }
        [Display(Name = "Quantity")]
        public int OrderQuantity { get; set; }
        public decimal Total { get; set; }

        public GameDrop_Product Product { get; set; }
    }
}
