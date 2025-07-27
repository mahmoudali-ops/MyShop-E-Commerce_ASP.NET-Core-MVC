using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccsess.Implementaion;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using MyShop.Web.ViewModels;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetData()
        {
            var orderheaders = unitOfWork.orderheader.GetAll(includeword: "applicationUser");
            return Json(new { data = orderheaders });
        }

        public IActionResult Details(int orderid)
        {
            OrderVM orderVM = new OrderVM()
            {
                orderHeader = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == orderid, includeword: "applicationUser"),
                orderDetails = unitOfWork.orderdetails.GetAll(e => e.OrderHeaderId == orderid, includeword: "product")
            };
            return View(orderVM);
        }
        public IActionResult UpdataOrderDetails()
        {
            var SelectedOrder = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == OrderVM.orderHeader.Id);
            if (SelectedOrder == null)
            {
                return NotFound();
            }
            SelectedOrder.Name = OrderVM.orderHeader.Name;
            SelectedOrder.City = OrderVM.orderHeader.City;
            SelectedOrder.Address = OrderVM.orderHeader.Address;
            SelectedOrder.PhoneNumber = OrderVM.orderHeader.PhoneNumber;

            if (OrderVM.orderHeader.Carrier != null)
            {
                SelectedOrder.Carrier = OrderVM.orderHeader.Carrier;
            }
            if (OrderVM.orderHeader.TrackingNumber != null)
            {
                SelectedOrder.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            }

            unitOfWork.orderheader.update(SelectedOrder);
            unitOfWork.Complete();
            return RedirectToAction("Details", "Order", new { orderid = SelectedOrder.Id });

        }

        [HttpPost]
        public IActionResult StartProcess()
        {

            unitOfWork.orderheader.updateStatus(OrderVM.orderHeader.Id, SD.Proccessing, null);
            var SelectedOrder = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == OrderVM.orderHeader.Id);

            unitOfWork.Complete();
            return RedirectToAction("Details", "Order", new { orderid = OrderVM.orderHeader.Id });

        }

        [HttpPost]
        public IActionResult StartShipping()
        {
            var SelectedOrder = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == OrderVM.orderHeader.Id);
            if (SelectedOrder == null)
            {
                return NotFound();
            }

            SelectedOrder.OrderStatus = SD.Shipped;
            SelectedOrder.Carrier = OrderVM.orderHeader.Carrier;
            SelectedOrder.TrackingNumber = OrderVM.orderHeader.TrackingNumber;
            SelectedOrder.ShippingDate = DateTime.Now;

            unitOfWork.orderheader.update(SelectedOrder);
            unitOfWork.Complete();
            return RedirectToAction("Details", "Order", new { orderid = SelectedOrder.Id });

        }

        [HttpPost]
        public IActionResult CancleOrder()
        {
            var SelectedOrder = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == OrderVM.orderHeader.Id);
            if (SelectedOrder.PaymentStatus == SD.Approve)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = SelectedOrder.PaymentInetId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);
                unitOfWork.orderheader.updateStatus(OrderVM.orderHeader.Id, SD.Cancled, SD.Refund);

            }
            else
            {
                unitOfWork.orderheader.updateStatus(OrderVM.orderHeader.Id, SD.Cancled, null);
            }

            unitOfWork.orderheader.update(SelectedOrder);
            unitOfWork.Complete();
            return RedirectToAction("Details", "Order", new { orderid = SelectedOrder.Id });


        }
    }
}