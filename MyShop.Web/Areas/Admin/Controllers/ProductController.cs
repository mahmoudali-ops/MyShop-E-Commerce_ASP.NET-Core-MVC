using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Utilities;
using MyShop.Web.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetData()
        {
            var products = _unitOfWork.product.GetAll(includeword: "category");
            return Json(new {data=products });
        }


            [HttpGet]
        public IActionResult Create()
        
        {
            ProductVM productVM = new ProductVM()
            {
                product = new Product(),
                CategoryList=_unitOfWork.category.GetAll().Select(e=> new SelectListItem 
                {
                    Text=e.Name,
                    Value = e.ID.ToString()
                })
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( ProductVM productVM,IFormFile file)
        {
            if (ModelState.IsValid) 
            {   
                string rootpath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload=Path.Combine(rootpath, @"images\products");
                    var ext=Path.GetExtension(file.FileName);

                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create)) 
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.product.Img = @"\images\products\" + filename + ext;

                }
                _unitOfWork.product.Add(productVM.product);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

           return View(productVM);  

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null | id == 0) return NotFound(); 

            ProductVM productVM = new ProductVM()
            {
                product = _unitOfWork.product.GetFirstorDefault(e => e.Id == id),
                CategoryList = _unitOfWork.category.GetAll().Select(e => new SelectListItem
                {
                    Text = e.Name,
                    Value = e.ID.ToString()
                })
            };
            return View(productVM);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootpath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootpath, @"images\products");
                    var ext = Path.GetExtension(file.FileName);
                    if (productVM.product.Img != null) 
                    {
                        var oldimg = Path.Combine(rootpath, productVM.product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    productVM.product.Img = @"\images\products\" + filename + ext;

                }
                // _context.categories.Update(Product);
                _unitOfWork.product.update(productVM.product);
                //_context.SaveChanges();
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }

            return View(productVM.product); 
        }

        [HttpDelete]
        public IActionResult Delete(int? id) 
        {
            if (id == null | id == 0) return NotFound();

            var deleted =  _unitOfWork.product.GetFirstorDefault(e => e.Id == id);
            if (deleted == null) 
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.product.Remove(deleted);
            
            var oldimg = Path.Combine(_webHostEnvironment.WebRootPath,deleted.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            _unitOfWork.Complete();

            return Json(new { success = true, message = "Deleted Successfully" });
        }



    }
}
