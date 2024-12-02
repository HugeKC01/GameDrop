using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameDrop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly GameDropDBContext _db;

        public ProductController(GameDropDBContext db)
        {
            _db = db;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products
                .Include(p => p.Category)
                .ToListAsync();
            return View(products);
        }



        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }



        // GET: Admin/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            return View();
        }



        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductImage,CategoryId,Quantity")] GameDrop_Product products)
        {
            if (ModelState.IsValid)
            {
                if (products.ProductImage != null && products.ProductImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        products.ProductImage.CopyTo(memoryStream);
                        products.ProductImageData = memoryStream.ToArray();
                        products.ProductImageType = products.ProductImage.ContentType;
                    }
                }
                _db.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }



        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _db.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }



        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductImageData,ProductImageType,CategoryId,Quantity")] GameDrop_Product products, IFormFile ProductImage)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            ModelState.Clear();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
                    if (ProductImage != null && ProductImage.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            ProductImage.CopyTo(memoryStream);
                            products.ProductImageData = memoryStream.ToArray();
                            products.ProductImageType = ProductImage.ContentType;
                        }
                    }
                    else
                    {
                        products.ProductImageData = existingProduct.ProductImageData;
                        products.ProductImageType = existingProduct.ProductImageType;
                    }
                    _db.Update(products);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductId))
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
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "CategoryName", products.CategoryId);
            return View(products);
        }



        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _db.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }



        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _db.Products.FindAsync(id);
            if (products != null)
            {
                _db.Products.Remove(products);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _db.Products.Any(e => e.ProductId == id);
        }
    }
}

