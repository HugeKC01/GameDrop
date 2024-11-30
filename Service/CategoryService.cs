using System.Collections.Generic;
using System.Linq;
using GameDrop.Models;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;

namespace GameDrop.Services
{
    public class CategoryService
    {
        private readonly GameDropDBContext _context;

        public CategoryService(GameDropDBContext context)
        {
            _context = context;
        }

        public List<GameDrop_Category> GetCategories()
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .ToList();
        }

        public GameDrop_Category? GetCategoryById(int categoryId)
        {
            return _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefault(c => c.CategoryId == categoryId);
        }

        public void AddCategory(GameDrop_Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void UpdateCategory(GameDrop_Category category)
        {
            var existingCategory = GetCategoryById(category.CategoryId);
            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.ParentCategoryId = category.ParentCategoryId;
                _context.SaveChanges();
            }
        }

        public void DeleteCategory(GameDrop_Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}