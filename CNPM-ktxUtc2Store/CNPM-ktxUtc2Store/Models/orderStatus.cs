using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("orderStatus")]
    public class orderStatus
    {
        public int Id { get; set; } 
        public string? statusName { get; set; }    
    }
}
