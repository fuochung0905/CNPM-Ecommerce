using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CNPM_ktxUtc2Store.Models
{
    [Table("InforStorage")]
    public class InforStorage
    {
        [Key]
        public int Id { get; set; }
        public string namestorage { get; set; }
        public string logo { get; set; }
        [NotMapped]
        [Display(Name = "choose Logo")]
        public IFormFile pictureLogo { get; set; }
        public string linkfacbook { get; set; }
        public string linkInstagram { get; set; }
        public string linkyoutube { get; set;}
        public string linktiktok { get; set; }
        public string phonenumbershop { get; set; }
        public string emailcskh { get; set; }   
        public string emailwork { get; set; }
        public string timework { get; set; }
        public virtual List<AdressStorage> AdressStorages { get; set; } = new List<AdressStorage>();

        public virtual List<BannerStorage> BannerStorages { get; set; } = new List<BannerStorage>();
    }
}
