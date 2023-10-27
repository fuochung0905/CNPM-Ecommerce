using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("order")]
    public class order
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public DateTime createDate { get; set; } = DateTime.UtcNow;
        public int orderStatusId { get; set; } 
        public bool IsDelete { get; set; }
        public virtual orderStatus status { get; set; }
       
        public virtual List<orderDetail> orderDetails { get; set; }
    }
}
