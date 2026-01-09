using Microsoft.EntityFrameworkCore;
using foldingGate.Models.DB;



namespace foldingGate.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> customers { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Product> products { get; set; }
    }
}
