using CampCorr.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CampCorr.Services
{
    public class SeedUserRoleInitial : ISeedUserRoleInitial
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedUserRoleInitial(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("Piloto").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Piloto";
                role.NormalizedName = "PILOTO";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Adm").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Adm";
                role.NormalizedName = "ADM";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
            if (!_roleManager.RoleExistsAsync("Master").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Master";
                role.NormalizedName = "MASTER";
                IdentityResult roleResult = _roleManager.CreateAsync(role).Result;
            }
        }

        //public void SeedUsers()
        //{
        //    if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
        //    {
        //        IdentityUser user = new IdentityUser();
        //        user.UserName = "usuario@localhost";
        //        user.Email = "usuario@localhost";
        //        user.NormalizedUserName = "USUARIO@LOCALHOST";
        //        user.NormalizedEmail = "USUARIO@LOCALHOST";
        //        user.EmailConfirmed = true;
        //        user.LockoutEnabled = true;
        //        user.SecurityStamp = Guid.NewGuid().ToString();

        //        IdentityResult result = _userManager.CreateAsync(user, "Numsey#2022").Result;

        //        if (result.Succeeded)
        //        {
        //            _userManager.AddToRoleAsync(user, "Member").Wait();
        //        }
        //    }

        //    if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
        //    {
        //        IdentityUser user = new IdentityUser();
        //        user.UserName = "admin@localhost";
        //        user.Email = "admin@localhost";
        //        user.NormalizedUserName = "ADMIN@LOCALHOST";
        //        user.NormalizedEmail = "ADMIN@LOCALHOST";
        //        user.EmailConfirmed = true;
        //        user.LockoutEnabled = true;
        //        user.SecurityStamp = Guid.NewGuid().ToString();

        //        IdentityResult result = _userManager.CreateAsync(user, "Numsey#2022").Result;

        //        if (result.Succeeded)
        //        {
        //            _userManager.AddToRoleAsync(user, "Admin").Wait();
        //        }
        //    }
        //}
    }
}
