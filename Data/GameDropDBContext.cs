using Microsoft.EntityFrameworkCore;
using GameDrop.Models;
using GameDrop.Services;

namespace GameDrop.Data
{
    public class GameDropDBContext : DbContext
    {
        public GameDropDBContext()
        {
        }

        public GameDropDBContext(DbContextOptions<GameDropDBContext> options) : base(options)
        {
        }

        public DbSet<GameDrop.Models.GameDrop_Category> Categories { get; set; }
        public DbSet<GameDrop.Models.GameDrop_Coupon> Coupons { get; set; }
        public DbSet<GameDrop.Models.GameDrop_OrderDetails> OrderDetails { get; set; }
        public DbSet<GameDrop.Models.GameDrop_Order> Orders { get; set; }
        public DbSet<GameDrop.Models.GameDrop_Product> Products { get; set; }
        public DbSet<GameDrop.Models.GameDrop_PromoBanner> PromoBanners { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<GameDrop.Models.GameDrop_UserAddress> GameDrop_UserAddress { get; set; } = default!;
    }
}