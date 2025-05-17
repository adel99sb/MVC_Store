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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<AdminSettings> AdminSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.ProductId);
                
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Cart>(c => c.UserId);
                
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId);
                
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.) // Define the navigation property
            //    .WithMany(u => u.) // Define the inverse relationship
            //    .HasForeignKey(o => o.UserId) // Define the foreign key
            //    .OnDelete(DeleteBehavior.Restrict); // Set ON DELETE behavior
        }

    }
}
