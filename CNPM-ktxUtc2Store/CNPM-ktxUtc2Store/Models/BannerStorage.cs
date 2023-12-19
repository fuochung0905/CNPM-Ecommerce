using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("bannerStorage")]
    public class BannerStorage
    {
        [Key]
        public int Id { get; set; }
        public string Bannerpicture { get; set; }
        [NotMapped]
        
        public IFormFile picture { get; set; }
       
        public int InforStorageId { get; set; }
        public InforStorage InforStorage { get; set; }
    }
}
