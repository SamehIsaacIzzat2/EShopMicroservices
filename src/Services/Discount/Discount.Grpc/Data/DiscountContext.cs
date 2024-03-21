using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DbSet<Coupon> Coupons { get; set; } = default!;

        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon() { Id = 1, Description = "Item 1 des", ProductName = "Iphone X", Amount = 2 },
                new Coupon() { Id = 2, Description = "Item 2 des", ProductName = "Iphone 15", Amount = 2 });
        }
    }
}
