﻿using CNPM_ktxUtc2Store.Controllers.Constants;
using CNPM_ktxUtc2Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CNPM_ktxUtc2Store.Data
{
    public class DbSeeder
    {

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
