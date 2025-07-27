using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public DashBoardController(IUnitOfWork _unitOfWork)
        {
           unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {

            ViewBag.Rev = unitOfWork.orderheader.GetAll(t => t.OrderStatus == SD.Approve).Sum(t => t.TotalCost);
            ViewBag.Product = unitOfWork.product.GetAll().Count();
            ViewBag.Users = unitOfWork.appUser.GetAll().Count();
            ViewBag.Orders= unitOfWork.orderheader.GetAll().Count();
            ViewBag.ApprovedOrders= unitOfWork.orderheader.GetAll(t=>t.OrderStatus==SD.Approve).Count();
            return View();
        }
    }
}
