using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameDrop.Data;
using GameDrop.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameDrop.Controllers
{
    [Authorize]
    public class PromoBannerController : Controller
    {
        private readonly GameDropDBContext _context;

        public PromoBannerController(GameDropDBContext context)
        {
            _context = context;
        }

        // GET: GameDrop_PromoBanner
        public async Task<IActionResult> Index()
        {
            return View(await _context.PromoBanners.ToListAsync());
        }

        public async Task<List<GameDrop_PromoBanner>> GetPromoBanners()
        {
            return await _context.PromoBanners.ToListAsync();
        }

        // GET: GameDrop_PromoBanner/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_PromoBanner = await _context.PromoBanners
                .FirstOrDefaultAsync(m => m.PromoBannerId == id);
            if (gameDrop_PromoBanner == null)
            {
                return NotFound();
            }

            return View(gameDrop_PromoBanner);
        }

        // GET: GameDrop_PromoBanner/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GameDrop_PromoBanner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PromoBannerId,PromoBannerName,PromoBannerDescription,PromoBannerImageData,PromoBannerImageType,BannerType")] GameDrop_PromoBanner PromoBanner, IFormFile PromoBannerImage)
        {
            if (ModelState.IsValid)
            {
                if (PromoBannerImage != null && PromoBannerImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await PromoBannerImage.CopyToAsync(memoryStream);
                        PromoBanner.PromoBannerImageData = memoryStream.ToArray();
                        PromoBanner.PromoBannerImageType = PromoBannerImage.ContentType; // Optional: store the MIME type
                    }
                }
                _context.Add(PromoBanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(PromoBanner);
        }

        // GET: GameDrop_PromoBanner/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_PromoBanner = await _context.PromoBanners.FindAsync(id);
            if (gameDrop_PromoBanner == null)
            {
                return NotFound();
            }
            return View(gameDrop_PromoBanner);
        }

        // POST: GameDrop_PromoBanner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PromoBannerId,PromoBannerName,PromoBannerDescription,PromoBannerImageData,PromoBannerImageType,BannerType")] GameDrop_PromoBanner gameDrop_PromoBanner)
        {
            if (id != gameDrop_PromoBanner.PromoBannerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameDrop_PromoBanner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameDrop_PromoBannerExists(gameDrop_PromoBanner.PromoBannerId))
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
            return View(gameDrop_PromoBanner);
        }

        // GET: GameDrop_PromoBanner/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_PromoBanner = await _context.PromoBanners
                .FirstOrDefaultAsync(m => m.PromoBannerId == id);
            if (gameDrop_PromoBanner == null)
            {
                return NotFound();
            }

            return View(gameDrop_PromoBanner);
        }

        // POST: GameDrop_PromoBanner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameDrop_PromoBanner = await _context.PromoBanners.FindAsync(id);
            if (gameDrop_PromoBanner != null)
            {
                _context.PromoBanners.Remove(gameDrop_PromoBanner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameDrop_PromoBannerExists(int id)
        {
            return _context.PromoBanners.Any(e => e.PromoBannerId == id);
        }
    }
}
