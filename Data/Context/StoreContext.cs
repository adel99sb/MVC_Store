using Ali_Store.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Ali_Store.Data.Context
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        public DbSet<_User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Oreder> Oreders { get; set; }

    }
}
