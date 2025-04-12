using Ali_Store.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Ali_Store.Data.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<_User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Rate> Rates { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.) // Define the navigation property
            //    .WithMany(u => u.) // Define the inverse relationship
            //    .HasForeignKey(o => o.UserId) // Define the foreign key
            //    .OnDelete(DeleteBehavior.Restrict); // Set ON DELETE behavior
        }

    }
}
