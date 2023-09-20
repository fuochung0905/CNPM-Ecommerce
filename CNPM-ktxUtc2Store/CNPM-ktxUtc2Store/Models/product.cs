using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("product")]
    public class product
    {
        public int Id { get; set; }
        [Required]

        public string? productName { get; set; }
        public string? description { get; set; }
        public double discount { get; set;}
        public double price { get; set; }
        public string?  imageUrl { get; set;}
        public int categoryId { get; set;}
        public category category { get; set; }
        public string categoryName { get; set; }
    }
}
