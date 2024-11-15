using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Coupon
    {
        [Key]
        [Required]
        public int CouponId { get; set; }
        public string? CouponName { get; set; }
        public string? CouponDescription { get; set; }
        public string? CouponCode { get; set; }
        public string? CouponType { get; set; }
        public string? CouponValue { get; set; }
    }
}
