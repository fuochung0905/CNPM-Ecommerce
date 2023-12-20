using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("cartDetail")]
    public class cartDetail
    {
        [Key]
        public int Id { get; set; }
        public int shoppingCartId { get; set; }
        public virtual shoppingCart shoppingCart { get; set; }
        public int productId { get; set; }
        public virtual product product { get; set; }

        public int quantity { get; set; }
        public double unitPrice { get; set; }

        public string size { get; set; }
        public string color { get; set; }
    }
}
