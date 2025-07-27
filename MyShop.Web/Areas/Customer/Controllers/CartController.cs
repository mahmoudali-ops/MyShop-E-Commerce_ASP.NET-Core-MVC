using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccsess.Implementaion;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using MyShop.Web.ViewModels;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Stripe.FinancialConnections;
using System.Security.Claims;
using Session = Stripe.Checkout.Session;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public ShopingCartVM  shopingCart { get; set; }
        public CartController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCart = new ShopingCartVM()
            {
                CartsList = unitOfWork.shopingcart.GetAll(c => c.ApplicationUserId == Claim.Value, includeword: "product")
            };

            foreach (var item in shopingCart.CartsList)
            {
                shopingCart.TotalCost += (item.Count * item.product.Price);
            }
            return View(shopingCart);
        }

        public IActionResult Plus(int cartid)
        {
            var ShopingCartIncreased = unitOfWork.shopingcart.GetFirstorDefault(c => c.Id == cartid);
            unitOfWork.shopingcart.IncreaseCount(ShopingCartIncreased, 1);
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult Mins(int cartid)
        {
            var ShopingCartDecreased = unitOfWork.shopingcart.GetFirstorDefault(c => c.Id == cartid);

            if (ShopingCartDecreased.Count <= 1)
            {
                unitOfWork.shopingcart.Remove(ShopingCartDecreased);
                var count = unitOfWork.shopingcart.GetAll(e => e.ApplicationUserId == ShopingCartDecreased.ApplicationUserId).ToList().Count()-1;
                HttpContext.Session.SetInt32(SD.SessionKey, count);
                unitOfWork.Complete();


            }
            else
            {
                unitOfWork.shopingcart.DecreaseCount(ShopingCartDecreased, 1);

            }
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int cartid)
        {
            var RemovedCart = unitOfWork.shopingcart.GetFirstorDefault(e => e.Id == cartid);
            unitOfWork.shopingcart.Remove(RemovedCart);
            unitOfWork.Complete();
            var count= unitOfWork.shopingcart.GetAll(e => e.ApplicationUserId == RemovedCart.ApplicationUserId).ToList().Count();
            HttpContext.Session.SetInt32(SD.SessionKey, count);
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Summry()
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCart = new ShopingCartVM()
            {
                CartsList = unitOfWork.shopingcart.GetAll(c => c.ApplicationUserId == Claim.Value, includeword: "product"),
                OrderHeader = new()

            };

            shopingCart.OrderHeader.applicationUser = unitOfWork.appUser.GetFirstorDefault(e => e.Id == Claim.Value);

            shopingCart.OrderHeader.Name = shopingCart.OrderHeader.applicationUser.Name;
            shopingCart.OrderHeader.Address = shopingCart.OrderHeader.applicationUser.Address;
            shopingCart.OrderHeader.City = shopingCart.OrderHeader.applicationUser.City;
            shopingCart.OrderHeader.PhoneNumber = shopingCart.OrderHeader.applicationUser.PhoneNumber;

            foreach (var item in shopingCart.CartsList)
            {
                shopingCart.TotalCost += (item.Count * item.product.Price);
            }
            return View(shopingCart);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Summry(ShopingCartVM shopingCartVM)
        {
            var ClaimIdentity = (ClaimsIdentity)User.Identity;
            var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shopingCartVM.CartsList = unitOfWork.shopingcart.GetAll(c => c.ApplicationUserId == Claim.Value, includeword: "product");

            shopingCartVM.OrderHeader.OrderStatus = SD.Pending;
            shopingCartVM.OrderHeader.PaymentStatus = SD.Pending;
            shopingCartVM.OrderHeader.OrderDate = DateTime.Now;
            shopingCartVM.OrderHeader.ApplicatioUserId = Claim.Value;

            foreach (var item in shopingCartVM.CartsList)
            {
                shopingCartVM.TotalCost += (item.Count * item.product.Price);
            }

            shopingCartVM.OrderHeader.TotalCost = shopingCartVM.TotalCost;

            unitOfWork.orderheader.Add(shopingCartVM.OrderHeader);
            unitOfWork.Complete();
            int newOrderId = shopingCartVM.OrderHeader.Id;

            foreach (var product in shopingCartVM.CartsList)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    Count = product.Count,
                    Price = product.product.Price,
                    ProductId = product.product.Id,
                    OrderHeaderId = shopingCartVM.OrderHeader.Id,
                };

                unitOfWork.orderdetails.Add(orderDetails);
                unitOfWork.Complete();

            }

            // Stripe
            var domain = "https://localhost:7045/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shopingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"Customer/Cart/Index",
            };

            foreach (var item in shopingCartVM.CartsList)
            {
                var sessionkineoptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.product.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionkineoptions);

            }

            var serivce = new SessionService();
            Session session = serivce.Create(options);
            shopingCartVM.OrderHeader.SessionId = session.Id;
            unitOfWork.orderheader.update(shopingCartVM.OrderHeader);
            unitOfWork.Complete();

            unitOfWork.shopingcart.RemoveRange(shopingCartVM.CartsList);
            unitOfWork.Complete();

            //  return RedirectToAction("OrderConfirmation", "Cart", new { id = newOrderId });

            //unitOfWork.Complete();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        #region summry_post 
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Summry(ShopingCartVM shopingCartVM)
        //{
        //    var ClaimIdentity = (ClaimsIdentity)User.Identity;
        //    var Claim = ClaimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    shopingCartVM.CartsList = unitOfWork.shopingcart.GetAll(c => c.ApplicationUserId == Claim.Value, includeword: "product");

        //    shopingCartVM.OrderHeader.OrderStatus = SD.Pending;
        //    shopingCartVM.OrderHeader.PaymentStatus = SD.Pending;
        //    shopingCartVM.OrderHeader.OrderDate = DateTime.Now;
        //    shopingCartVM.OrderHeader.ApplicatioUserId = Claim.Value;

        //    foreach (var item in shopingCartVM.CartsList)
        //    {
        //        shopingCartVM.TotalCost += (item.Count * item.product.Price);
        //    }

        //    unitOfWork.orderheader.Add(shopingCartVM.OrderHeader);
        //    unitOfWork.Complete();

        //    foreach (var product in shopingCartVM.CartsList)
        //    {
        //        OrderDetails orderDetails = new OrderDetails()
        //        {
        //            Count = product.Count,
        //            Price = product.product.Price,
        //            ProductId = product.product.Id,
        //            OrderHeaderId = shopingCartVM.OrderHeader.Id,
        //        };

        //        unitOfWork.orderdetails.Add(orderDetails);
        //        unitOfWork.Complete();

        //    }

        //    // Stripe
        //    var domain = "https://localhost:7045/";
        //    var options = new SessionCreateOptions
        //    {
        //        LineItems = new List<SessionLineItemOptions>(),
        //        Mode = "payment",
        //        SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shopingCartVM.OrderHeader.Id}",
        //        CancelUrl = domain + $"Customer/Cart/Index",
        //    };

        //    foreach (var item in shopingCartVM.CartsList)
        //    {
        //        var sessionkineoptions = new SessionLineItemOptions
        //        {
        //            PriceData = new SessionLineItemPriceDataOptions
        //            {
        //                UnitAmount = (long)(item.product.Price * 100),
        //                Currency = "usd",
        //                ProductData = new SessionLineItemPriceDataProductDataOptions
        //                {
        //                    Name = item.product.Name,
        //                },
        //            },
        //            Quantity = item.Count,
        //        };
        //        options.LineItems.Add(sessionkineoptions);

        //    }

        //    var serivce = new SessionService();
        //    Session session = serivce.Create(options);
        //    shopingCartVM.OrderHeader.SessionId = session.Id;


        //    unitOfWork.shopingcart.RemoveRange(shopingCartVM.CartsList);
        //    unitOfWork.Complete();
        //    return RedirectToAction("Index", "Home");

        //    //unitOfWork.Complete();
        //    //Response.Headers.Add("Location", session.Url);
        //    //return new StatusCodeResult(303);

        //}
        #endregion

        public IActionResult OrderConfirmation(int id) 
        {
            Console.WriteLine("Trying to fetch OrderHeader with id = " + id);
            var all = unitOfWork.orderheader.GetAll().ToList();

            OrderHeader orderHeader = all.FirstOrDefault(e => e.Id == id);

           //
           //OrderHeader orderHeader = unitOfWork.orderheader.GetFirstorDefault(e => e.Id == id);
            var service=new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower()=="paid") 
            {
                unitOfWork.orderheader.updateStatus(id, SD.Approve, SD.Approve);
              //  orderHeader.OrderStatus = SD.Approve;
                //orderHeader.PaymentStatus = SD.Approve;
                orderHeader.PaymentInetId = session.PaymentIntentId;
                unitOfWork.orderheader.update(orderHeader);
                unitOfWork.Complete();
            }
            List<ShopingCart> shopingCart =unitOfWork.shopingcart.GetAll(e=>e.ApplicationUserId==orderHeader.ApplicatioUserId).ToList();
            unitOfWork.shopingcart.RemoveRange(shopingCart);
            unitOfWork.Complete();
            return View();
        }



    } 
}
