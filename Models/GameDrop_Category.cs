using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDrop.Models
{
    public class GameDrop_Category
    {
        [Key]
        [Required]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        [ForeignKey("ParentCategoryId")]
        public GameDrop_Category? ParentCategory { get; set; }
        public ICollection<GameDrop_Category>? SubCategories { get; set; }

        public List<GameDrop_Category> GetAllSubCategories()
        {
            var allSubCategories = new List<GameDrop_Category>();
            if (SubCategories != null)
            {
                foreach (var subCategory in SubCategories)
                {
                    allSubCategories.Add(subCategory);
                    allSubCategories.AddRange(subCategory.GetAllSubCategories());
                }
            }
            return allSubCategories;
        }
    }
}
