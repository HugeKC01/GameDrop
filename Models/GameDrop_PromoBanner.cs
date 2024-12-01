using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_PromoBanner
    {
        [Key]
        [Required]
        public int PromoBannerId { get; set; }
        public string? PromoBannerName { get; set; }
        public string? PromoBannerDescription { get; set; }

        [NotMapped]
        [Display(Name = "BannerImage")]
        public IFormFile? PromoBannerImage { get; set; }
        public byte[]? PromoBannerImageData { get; set; }
        public string? PromoBannerImageType { get; set; }
        public string? BannerType { get; set; }
    }
}
