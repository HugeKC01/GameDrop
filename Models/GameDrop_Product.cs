using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Product
    {
        [Key]
        [Required]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }

        [NotMapped]
        [Display(Name = "Product Image")]
        public IFormFile? ProductImage { get; set; }
        public byte[]? ProductImageData { get; set; }
        public string? ProductImageType { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }

    }
}
