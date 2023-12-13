using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("product")]
    public class product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? productName { get; set; }
        public string? description { get; set; }
        public double oldprice { get; set;}
        public double price { get; set; }
        public string?  imageUrl { get; set;}
        [NotMapped]
        [Display(Name ="choose image")]
        public IFormFile image { get; set; }
        public int categoryId { get; set;}
        public virtual category category { get; set; }
        public int qty_inStock { get; set; } = 0;

        public virtual List<productVariation> ProductVariations { get; set; }  = new List<productVariation>();
      
    }
}
