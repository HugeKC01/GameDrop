using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameDrop.Data;
using GameDrop.Models;

namespace GameDrop.Services
{
    public class PromoBannerService
    {
        private readonly GameDropDBContext _context;

        public PromoBannerService(GameDropDBContext context)
        {
            _context = context;
        }

        public async Task<List<GameDrop_PromoBanner>> GetPromoBannersAsync()
        {
            return await _context.PromoBanners.ToListAsync();
        }
    }
}