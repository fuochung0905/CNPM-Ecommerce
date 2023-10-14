using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("variation")]
    public class variation
    {
        public int Id { get; set; }
        public string name { get; set; }
        public ICollection<variation_option>variation_Options { get; set; }
    }
}
