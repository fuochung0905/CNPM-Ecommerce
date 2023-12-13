using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("adressStorage")]
    public class AdressStorage
    {
        [Key]
        public int Id { get; set; } 
        public string NameAdress { get; set; }
        public int InforStorageId { get; set; } 
        public InforStorage InforStorage { get; set; }
    }
}
