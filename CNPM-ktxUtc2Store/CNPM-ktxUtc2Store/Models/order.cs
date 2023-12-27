using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("order")]
    public class order
    {
        [Key]
        public int Id { get; set; }
        public DateTime createDate { get; set; } = DateTime.Now;
        public DateTime updateDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsComplete { get; set; }
        public bool isHuy { get; set; }
        public int orderStatusId { get; set; }
        public virtual orderStatus status { get; set; }
        public string applicationUserId { get; set; }
        public virtual applicationUser applicationUser { get; set; }
        public virtual List<orderDetail> orderDetails { get; set; }
    }
}
