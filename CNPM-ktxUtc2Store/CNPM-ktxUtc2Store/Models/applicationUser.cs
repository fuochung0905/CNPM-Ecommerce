using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CNPM_ktxUtc2Store.Models
{
    public class applicationUser :IdentityUser
    {
        public string fullname { get; set; }    
        public string profilePicture {  get; set; }
        [NotMapped]
        [Display(Name = "choose Picture")]
        public IFormFile Picture { get; set; }
        public virtual List<UserAdress> UserAdresses { get; set; } = new List<UserAdress>();

    }
}
