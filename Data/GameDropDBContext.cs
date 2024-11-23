﻿using Microsoft.EntityFrameworkCore;
using GameDrop.Models;

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
        public DbSet<GameDrop.Models.GameDrop_ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints if necessary
            modelBuilder.Entity<GameDrop_OrderDetails>()
                .HasOne<GameDrop_Order>()
                .WithMany()
                .HasForeignKey(od => od.OrderId);

            modelBuilder.Entity<GameDrop_OrderDetails>()
                .HasOne<GameDrop_Product>()
                .WithMany()
                .HasForeignKey(od => od.ProductId);

            modelBuilder.Entity<GameDrop_Product>()
                .HasOne<GameDrop_Category>()
                .WithMany()
                .HasForeignKey(p => p.CategoryId);
        }
    }
}