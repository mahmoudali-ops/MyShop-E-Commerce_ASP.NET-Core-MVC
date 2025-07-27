using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private ApplicationDbContext context;

        public UsersController(ApplicationDbContext _context)
        {
            context = _context;
        }
     
        public IActionResult Index()
        {
            
                var ClaimIdentity = (ClaimsIdentity)User.Identity;
                var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var UserId = Claim.Value;
                return View(context.ApplicationUsers.Where(u => u.Id != UserId));
          
        }

        public IActionResult LockUnLock(string? id) 
        {
           var user =context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (user == null) 
            {
                return NotFound();
            }
            if (user.LockoutEnd == null || user.LockoutEnd <= DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddDays(30);
            }
            else 
            {
                user.LockoutEnd =null;
            }

            context.ApplicationUsers.Update(user); 
            context.SaveChanges();
            return RedirectToAction("Index", "Users", new {area="Admin"});
            
        }
    }
}
