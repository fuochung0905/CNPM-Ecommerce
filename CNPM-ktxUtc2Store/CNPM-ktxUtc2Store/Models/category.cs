using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("category")]
    public class category
    {
        public int Id { get; set; }
        public string categoryName { get; set; }
        public List<product> products { get; set; }
    }
}
