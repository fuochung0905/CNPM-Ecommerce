using CNPM_ktxUtc2Store.Controllers.Constants;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CNPM_ktxUtc2Store.Data
{
    public class DbSeeder
    {
       
      

        public static async Task SeedInfor(IServiceProvider service)
        {

            InforStorage inforStorage= new InforStorage();
            inforStorage.namestorage = "UTC2";
            inforStorage.logo = "hihi";
            inforStorage.phonenumbershop = "0399333643";
            inforStorage.emailcskh = "6251071033@st.utc2.edu.vn";
            inforStorage.emailwork = "phuochungnguyen.work@gmail.com";
            inforStorage.timework="7:00-22:00";
            inforStorage.linkfacbook = "https://facbook.com";
            inforStorage.linkInstagram = "https://instagram.com";
            inforStorage.linkyoutube = "https://youtobe.com";
            inforStorage.linktiktok = "https://tiktok.com";
            
            var context = service.GetService<ApplicationDbContext>();

            var infor=context.InforStorage.Where(x=>x.namestorage==inforStorage.namestorage).FirstOrDefault();   
            if(infor==null)
            {
                context.InforStorage.Add(inforStorage);
                context.SaveChanges();
            }
           
        }
        public static async Task SeedRoleAndAdmin(IServiceProvider service)
        {
            // Seed Role
            var userManager = service.GetService<UserManager<applicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Saler.ToString()));


            // create Admin
            var user = new applicationUser
            {
                fullname = "Nguyễn Phước Hùng",
                profilePicture = "anhAdmin.jpg",
                UserName = "Admin123@gmail.com",
                Email = "Admin123@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
         

            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(user, "Admin@123");
                await userManager.AddToRoleAsync(user, Roles.Admin.ToString());

            }
        }
    }
}
