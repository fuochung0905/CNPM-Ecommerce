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
        public int categoryId { get; set; }
        public category category { get; set; } 
       
        public virtual ICollection<variation_option>variation_Options { get; set; }
    }
}
