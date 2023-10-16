using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("variation_option")]
    public class variation_option
    {
        [Key]
        public int Id { get; set; }
        public string value { get; set; }
        public int variationId { get; set; }
        public variation variation { get; set; }
        public virtual ICollection<productvoption> productvariation_Options { get; set; }
    }
}
