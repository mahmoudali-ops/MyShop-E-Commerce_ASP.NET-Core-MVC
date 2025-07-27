using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Entities.Models;

namespace MyShop.Web.ViewModels
{
    public class ProductVM
    {
        public Product product { get; set; }
       
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
