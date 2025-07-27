using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using System.Security.Claims;

namespace MyShop.Web.ViewComponents
{
    public class ShoppingCartVC : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;
        public ShoppingCartVC(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (Claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionKey,
                        unitOfWork.shopingcart.GetAll(
                            e => e.ApplicationUserId == Claim.Value).ToList().Count());
                    return View(HttpContext.Session.GetInt32(SD.SessionKey));
                }
            }
            else 
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            
        }

    }
}
