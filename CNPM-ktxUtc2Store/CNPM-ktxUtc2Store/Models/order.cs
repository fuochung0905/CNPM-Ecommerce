using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("order")]
    public class order
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public DateTime createDate { get; set; } = DateTime.UtcNow;
        public int orderStatusId { get; set; }
        public bool IsDelete { get; set; }
        public orderStatus status { get; set; }
        public List<orderDetail> orderDetails { get; set; }
    }
}
