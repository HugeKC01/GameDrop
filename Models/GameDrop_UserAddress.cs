using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDrop.Models
{
    public class GameDrop_UserAddress
    {
        [Key]
        [Required]
        public int UserAddressId { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string? UserAddress { get; set; }
        [Required]
        [Display(Name = "City")]
        public string? UserCity { get; set; }
        [Required]
        [Display(Name = "State / Province")]
        public string? UserState { get; set; }
        [Required]
        [Display(Name = "Zip Code / Postal Code")]
        public string? UserZip { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string? UserCountry { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string? UserPhone { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string? UserEmail { get; set; }
        public string? UserId { get; set; }
    }
}
