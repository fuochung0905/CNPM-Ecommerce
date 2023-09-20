using Microsoft.AspNetCore.Identity;

namespace CNPM_ktxUtc2Store.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? fullName { get; set; }   
        public string? imageUrl { get; set; }  
    }
}
