using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccsess.DBIInitliazer
{
    public class DBIInitliazer : IDBIInitliazer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext Context;
        public DBIInitliazer(

            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext _Context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            Context = _Context;
        }
        public void Initialize()
        {
            //  migrations
            try
            {
                if(Context.Database.GetMigrations().Count() > 0)
                {
                    Context.Database.Migrate();
                }
            }
            catch (Exception)
            {

                throw;
            }

            // roles
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new ApplicationUser
                {
                    Name = "Administrator",
                    UserName = "Admin@myshop.com",
                    Address = "Admin Address",
                    City = "Admin City",
                    PhoneNumber = "1234567890",
                    Email = "Admin@gmail.com"
                }, "123@mM");


                var AdminUser = Context.ApplicationUsers.FirstOrDefault(u => u.Email == "Admin@gmail.com");
            
                _userManager.AddToRoleAsync(AdminUser,SD.AdminRole).GetAwaiter().GetResult();
            }
            return;
        }
    }
}
