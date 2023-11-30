using Microsoft.AspNetCore.Identity;

namespace CNPM_ktxUtc2Store.Models
{
    public class applicationUser :IdentityUser
    {
        public string fullname { get; set; }    
        public string profilePicture {  get; set; } 
           


    }
}
