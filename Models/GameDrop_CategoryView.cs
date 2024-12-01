using GameDrop.Models;
using System.Collections.Generic;

namespace GameDrop.ViewModels
{
    public class CategoryViewModel
    {
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? ParentCategoryId { get; set; }
        public List<GameDrop_Category>? Categories { get; set; }
    }
}
