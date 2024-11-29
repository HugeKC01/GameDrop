using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Order
    {
        [Key]
        [Required]
        public int OrderId { get; set; }
        
        [Display(Name = "Order Date")]
        public string? OrderDate { get; set; }
        [Display(Name = "Status")]
        public string? OrderStatus { get; set; }
        [Display(Name = "Address")]
        public string? OrderAddress { get; set; }
        [Display(Name = "Phone Number")]
        public string? OrderPhone { get; set; }
        [Display(Name = "Email")]
        public string? OrderEmail { get; set; }
    }
}
