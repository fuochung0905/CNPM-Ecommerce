using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("variation")]
    public class variation
    {
        [Key]
        public int Id { get; set; }
        public string name { get; set; }
        public string value { get; set; }   
        public int categoryId { get; set; }
        public virtual category category { get; set; } 
        public virtual ICollection<productVariation> ProductVariations { get; set; }  
    }
}
