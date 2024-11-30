using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameDrop.Data;
using GameDrop.Models;

namespace GameDrop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly GameDropDBContext _context;

        public CategoryController(GameDropDBContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var gameDropDBContext = _context.Categories.Include(g => g.ParentCategory);
            return View(await gameDropDBContext.ToListAsync());
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_Category = await _context.Categories
                .Include(g => g.ParentCategory)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (gameDrop_Category == null)
            {
                return NotFound();
            }

            return View(gameDrop_Category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId");
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,ParentCategoryId")] GameDrop_Category gameDrop_Category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameDrop_Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", gameDrop_Category.ParentCategoryId);
            return View(gameDrop_Category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_Category = await _context.Categories.FindAsync(id);
            if (gameDrop_Category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", gameDrop_Category.ParentCategoryId);
            return View(gameDrop_Category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,ParentCategoryId")] GameDrop_Category gameDrop_Category)
        {
            if (id != gameDrop_Category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameDrop_Category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameDrop_CategoryExists(gameDrop_Category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryId", gameDrop_Category.ParentCategoryId);
            return View(gameDrop_Category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_Category = await _context.Categories
                .Include(g => g.ParentCategory)
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (gameDrop_Category == null)
            {
                return NotFound();
            }

            return View(gameDrop_Category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameDrop_Category = await _context.Categories.FindAsync(id);
            if (gameDrop_Category != null)
            {
                _context.Categories.Remove(gameDrop_Category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameDrop_CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
