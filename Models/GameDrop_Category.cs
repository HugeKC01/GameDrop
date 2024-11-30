using System.ComponentModel.DataAnnotations;

namespace GameDrop.Models
{
    public class GameDrop_Category
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public GameDrop_Category ParentCategory { get; set; }
        public ICollection<GameDrop_Category> SubCategories { get; set; }

        public GameDrop_Category()
        {
            SubCategories = new List<GameDrop_Category>();
        }
    }
}
