using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("variation_option")]
    public class variation_option
    {
        public int Id { get; set; }
        public string value { get; set; }
        public ICollection<product>products { get; set; }
    }
}
