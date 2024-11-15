using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Category
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
