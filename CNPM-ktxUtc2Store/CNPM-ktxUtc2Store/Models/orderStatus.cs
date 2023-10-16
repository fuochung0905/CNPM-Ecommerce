using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("orderStatus")]
    public class orderStatus
    {
        [Key]
        public int Id { get; set; } 
        public string? statusName { get; set; }    
    }
}
