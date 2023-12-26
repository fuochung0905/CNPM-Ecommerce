using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("category")]
    public class category
    {
        [Key]
        public int Id { get; set; }
        public string categoryName { get; set; }
        public virtual ICollection<product> products { get; set; }
        public virtual List<variation> variations { get; set; } = new List<variation>();
    }
}
