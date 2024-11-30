using Microsoft.AspNetCore.Mvc;
using GameDrop.Services;
using GameDrop.Models;
using GameDrop.ViewModels;

namespace GameDrop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = _categoryService.GetCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            var model = new CategoryViewModel
            {
                Categories = _categoryService.GetCategories()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = new GameDrop_Category
                {
                    CategoryName = model.CategoryName,
                    ParentCategoryId = model.ParentCategoryId
                };
                _categoryService.AddCategory(category);
                return RedirectToAction("Index");
            }
            model.Categories = _categoryService.GetCategories();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                ParentCategoryId = category.ParentCategoryId,
                Categories = _categoryService.GetCategories()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(CategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryService.GetCategoryById(model.CategoryId.Value);
                if (category == null)
                {
                    return NotFound();
                }

                category.CategoryName = model.CategoryName;
                category.ParentCategoryId = model.ParentCategoryId;
                _categoryService.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            model.Categories = _categoryService.GetCategories();
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}