using Microsoft.EntityFrameworkCore;
using Task.EF.Models;

namespace Task.EF
{
    public class CoreDbContext:DbContext
    {
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options) { }

        public DbSet<User>Users { get; set; }
        public DbSet<Product>Products { get; set; }
    }
}
