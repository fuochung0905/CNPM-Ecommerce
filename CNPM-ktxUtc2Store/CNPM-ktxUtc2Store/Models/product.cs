using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("product")]
    public class product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Nhập tên đi đồ ngu")]
        public string? productName { get; set; }
        public string? description { get; set; }
        [Range(0, 10000000, ErrorMessage = "Vui lòng nhập số không âm.")]
        [Required(ErrorMessage = "Nhập tên đi đồ ngu")]
        public double? oldprice { get; set; }
        [Range(0, 10000000, ErrorMessage = "Vui lòng nhập số không âm.")]
        [Required(ErrorMessage = "Nhập tên đi đồ ngu")]
        public double? price { get; set; }
        public int soluongnhap { get; set; }

        public string? imageUrl { get; set; }
        [NotMapped]
        [Display(Name = "choose image")]
        public IFormFile? image { get; set; }
        public int categoryId { get; set; }
        public virtual category? category { get; set; }

        [Range(0, 5000000, ErrorMessage = "Vui lòng nhập số không âm.")]
        [Required(ErrorMessage = "Nhập tên đi đồ ngu")]
        public int qty_inStock { get; set; } = 0;


        public int daban { get; set; } = 0;
        public virtual List<productVariation> ProductVariations { get; set; } = new List<productVariation>();

    }
}
