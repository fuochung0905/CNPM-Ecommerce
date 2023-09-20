using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("shoppingCart")]
    public class shoppingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool isDelete { get; set; }=false;
        public ICollection<cartDetail> cartDetails { get; set; }
    }
}
