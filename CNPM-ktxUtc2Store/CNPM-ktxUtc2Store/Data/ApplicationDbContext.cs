using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CNPM_ktxUtc2Store.Data
{
    public class ApplicationDbContext : IdentityDbContext<applicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<category> categories { get; set; }
        public DbSet<product> products { get; set; }
        public DbSet<cartDetail> cartDetails { get; set; }
        public DbSet<order> orders { get; set; }
        public DbSet<orderDetail> orderDetails { get; set; }
        public DbSet<orderStatus> orderStatus { get; set; }
        public DbSet<shoppingCart> shoppingCarts { get; set; }
        public DbSet<variation> variation { get; set; }
        public DbSet<productVariation> productVariations { get; set; }
        public DbSet<applicationUser> applicationUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
           
            builder.Entity<productVariation>().HasKey(pv => new { pv.productId, pv.variationId });
            builder.Entity<productVariation>()
                .HasOne(pv => pv.product).WithMany(pv => pv.ProductVariations).HasForeignKey(p => p.productId).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<productVariation>()
              .HasOne(pv => pv.variation).WithMany(pv => pv.ProductVariations).HasForeignKey(p => p.variationId).OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}