using System.Collections.Generic;
using GameDrop.Models;

namespace GameDrop.ViewModels
{
    public class SearchViewModel
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public List<GameDrop_Category>? Categories { get; set; }
        public List<GameDrop_Product>? Products { get; set; }
    }
}