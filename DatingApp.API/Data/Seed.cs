using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void SeedUsers()
        {
            if (_userManager.Users != null && _userManager.Users.Any())
                return;

            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            var roles = new List<Role> 
            {
                new Role { Name = "Member" },
                new Role { Name = "Admin" },
                new Role { Name = "Moderator" },
                new Role { Name = "VIP" },
            };

            foreach (var role in roles) 
            {
                _roleManager.CreateAsync(role).Wait();
            }

            foreach (var user in users)
            {
                user.Photos.SingleOrDefault().IsApproved = true;
                _userManager.CreateAsync(user, "password").Wait();
                _userManager.AddToRoleAsync(user, "Member").Wait();
            }

            var adminUser = new User {
                UserName = "Admin"
            };

            IdentityResult result = _userManager.CreateAsync(adminUser, "password").Result;

            if (result.Succeeded)
            {
                var admin = _userManager.FindByNameAsync("Admin").Result;
                _userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
            }
        }
    }
}