using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("orderDetail")]
    public class orderDetail
    {
        public int Id { get; set; }
        public int quantity { get; set; }
        public int unitPrice { get; set; }
        public int orderId { get; set;}
        public order order { get; set;} 
        public int productId { get; set; }
        public product product { get; set; }    
    }
}
