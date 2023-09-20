using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("cartDetail")]
    public class cartDetail
    {
        public int Id { get; set; }
        public int shoppingCartId { get; set; }
        public shoppingCart shoppingCart { get; set; }
        public int productId { get; set; }
        public product product { get; set; }
        public int quantity { get; set; }
    }
}
