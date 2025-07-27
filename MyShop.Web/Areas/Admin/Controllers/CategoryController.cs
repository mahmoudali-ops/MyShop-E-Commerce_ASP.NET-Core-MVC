using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Utilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var category = _unitOfWork.category.GetAll();
            return View(category);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {

            //_context.categories.Add(category);
            _unitOfWork.category.Add(category);
            //_context.SaveChanges();
            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0) return NotFound();

            var categroy = _unitOfWork.category.GetFirstorDefault(e => e.ID == id);
            return View(categroy);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {

            // _context.categories.Update(category);
            _unitOfWork.category.update(category);
            //_context.SaveChanges();
            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }

        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0) return NotFound();

            var deleted = _unitOfWork.category.GetFirstorDefault(e => e.ID == id);
            _unitOfWork.category.Remove(deleted);
            _unitOfWork.Complete();
            // _context.categories.Remove(deleted);
            //_context.SaveChanges();
            return RedirectToAction("Index");
        }




    }
}
