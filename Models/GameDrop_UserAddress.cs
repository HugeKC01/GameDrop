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
        public string? UserAddress { get; set; }
        [Required]
        public string? UserCity { get; set; }
        [Required]
        public string? UserState { get; set; }
        [Required]
        public string? UserZip { get; set; }
        [Required]
        public string? UserCountry { get; set; }
        [Required]
        public string? UserPhone { get; set; }
        [Required]
        public string? UserEmail { get; set; }
        public string? UserId { get; set; }
    }
}
