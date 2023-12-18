using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("orderDetail")]
    public class orderDetail
    {
        [Key]
        public int Id { get; set; }
        public int quantity { get; set; }
        public string size { get; set; }    
        public string color { get; set; }
        public double unitPrice { get; set; }
        public int orderId { get; set;}
        public virtual order order { get; set;} 
        public int productId { get; set; }
        public virtual product product { get; set; }    
        public string addressuer { get; set; }
   

    }
}
