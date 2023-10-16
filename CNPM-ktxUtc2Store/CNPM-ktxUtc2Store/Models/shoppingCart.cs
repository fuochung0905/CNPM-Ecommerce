using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("shoppingCart")]
    public class shoppingCart
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public bool isDelete { get; set; }=false;
        public virtual ICollection<cartDetail> cartDetails { get; set; }
    }
}
