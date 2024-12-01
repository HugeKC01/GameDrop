using Microsoft.AspNetCore.Mvc;
using GameDrop.Data;
using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameDrop.Controllers
{
    //[Authorize]
    public class ProductController : Controller
    {
        private readonly GameDropDBContext _db;

        public ProductController(GameDropDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Shop()
        {
            var products = await _db.Products.ToListAsync();
            return View(products);
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            var products = await _db.Products.ToListAsync();
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
            return View();
        }



        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductImage,Quantity")] GameDrop_Product products)
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
            return View(products);
        }



        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescription,ProductPrice,ProductImage,Quantity")] GameDrop_Product products)
        {
            if (id != products.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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

