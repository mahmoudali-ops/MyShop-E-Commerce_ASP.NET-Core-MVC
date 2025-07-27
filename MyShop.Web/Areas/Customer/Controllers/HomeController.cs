using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using MyShop.Web.ViewModels;
using System.Security.Claims;
using X.PagedList;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            var PageNumber=page ?? 1;
            int PageSize = 4;

            var products = _unitOfWork.product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }

        public IActionResult Details(int id) 
        {
            ShopingCart prdVM= new ShopingCart()
            {
                ProdductId = id,
                product = _unitOfWork.product.GetFirstorDefault( p => p.Id == id, includeword: "category"),
                Count = 1
            };
            if (prdVM == null)
            {
                return NotFound();
            }
            return View(prdVM);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]

        public IActionResult Details(ShopingCart shopingCart)
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shopingCart.ApplicationUserId= Claim.Value;

            ShopingCart ShopObj = _unitOfWork.shopingcart.GetFirstorDefault(
                e=>e.ApplicationUserId==shopingCart.ApplicationUserId && e.ProdductId==shopingCart.ProdductId);

            if (ShopObj == null)
            {
                _unitOfWork.shopingcart.Add(shopingCart);
                _unitOfWork.Complete();

                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.shopingcart.GetAll(e => e.ApplicationUserId == Claim.Value).ToList().Count()
                    );
                _unitOfWork.Complete();

            }
            else 
            {
                _unitOfWork.shopingcart.IncreaseCount(ShopObj,shopingCart.Count);
                _unitOfWork.Complete();

            }


            return RedirectToAction("Index");
        }
    }
}
