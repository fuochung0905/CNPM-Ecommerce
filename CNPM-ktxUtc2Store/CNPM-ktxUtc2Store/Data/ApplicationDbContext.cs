using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CNPM_ktxUtc2Store.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<category> categories { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<cartDetail> cartDetails { get;set; }
        public DbSet<order> orders { get; set; }
        public DbSet<orderDetail> orderDetails { get; set; }
        public DbSet<orderStatus> orderStatus { get; set; } 
        public DbSet<shoppingCart> shoppingCarts { get; set; }
    }
}