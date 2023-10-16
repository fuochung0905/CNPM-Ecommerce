using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<cartDetail> cartDetails { get; set; }
        public DbSet<order> orders { get; set; }
        public DbSet<orderDetail> orderDetails { get; set; }
        public DbSet<orderStatus> orderStatus { get; set; }
        public DbSet<shoppingCart> shoppingCarts { get; set; }
        public DbSet<variation> variation { get; set; }
        public DbSet<variation_option> variation_option { get; set; }
        public DbSet<productvoption> productvoption { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<productvoption>()
                 .HasKey(c => new { c.productId, c.variationoptionId });
            modelBuilder.Entity<productvoption>()
                .HasOne(b => b.product)
                .WithMany(ba => ba.productvariation_Options)
                .HasForeignKey(bi => bi.productId);
            modelBuilder.Entity<productvoption>()
              .HasOne(b => b.variationoption)
              .WithMany(ba => ba.productvariation_Options)
              .HasForeignKey(bi => bi.variationoptionId);
        
    } 

        }
}