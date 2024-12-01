using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameDrop.Data;
using GameDrop.Models;
using System.Security.Claims;

namespace GameDrop.Controllers
{
    public class UserAddressController : Controller
    {
        private readonly GameDropDBContext _context;

        public UserAddressController(GameDropDBContext context)
        {
            _context = context;
        }

        // GET: UserAddress
        public async Task<IActionResult> Index()
        {
            // Get the user ID from the identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Filter the addresses based on the user ID
            var userAddresses = await _context.GameDrop_UserAddress
                .Where(address => address.UserId == userId)
                .ToListAsync();

            return View(userAddresses);
        }

        // GET: UserAddress/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserAddress/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserAddressId,UserAddress,UserCity,UserState,UserZip,UserCountry,UserPhone,UserEmail")] GameDrop_UserAddress gameDrop_UserAddress)
        {
            if (ModelState.IsValid)
            {
                // Get the user ID from the identity
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                gameDrop_UserAddress.UserId = userId;

                _context.Add(gameDrop_UserAddress);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gameDrop_UserAddress);
        }

        // GET: UserAddress/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_UserAddress = await _context.GameDrop_UserAddress.FindAsync(id);
            if (gameDrop_UserAddress == null)
            {
                return NotFound();
            }
            return View(gameDrop_UserAddress);
        }

        // POST: UserAddress/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserAddressId,UserAddress,UserCity,UserState,UserZip,UserCountry,UserPhone,UserEmail,UserId")] GameDrop_UserAddress gameDrop_UserAddress)
        {
            if (id != gameDrop_UserAddress.UserAddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameDrop_UserAddress);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameDrop_UserAddressExists(gameDrop_UserAddress.UserAddressId))
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
            return View(gameDrop_UserAddress);
        }

        // GET: UserAddress/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameDrop_UserAddress = await _context.GameDrop_UserAddress
                .FirstOrDefaultAsync(m => m.UserAddressId == id);
            if (gameDrop_UserAddress == null)
            {
                return NotFound();
            }

            return View(gameDrop_UserAddress);
        }

        // POST: UserAddress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameDrop_UserAddress = await _context.GameDrop_UserAddress.FindAsync(id);
            if (gameDrop_UserAddress != null)
            {
                _context.GameDrop_UserAddress.Remove(gameDrop_UserAddress);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameDrop_UserAddressExists(int id)
        {
            return _context.GameDrop_UserAddress.Any(e => e.UserAddressId == id);
        }
    }
}
