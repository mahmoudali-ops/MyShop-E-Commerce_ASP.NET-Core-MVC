using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyShop.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Web.ViewModels
{
    public class ShopingCart
    {

        [BindNever]
        public int Id { get; set; }
        public int ProdductId { get; set; }

        [ForeignKey("ProdductId")]
        public Product product { get; set; }

        [Range(1,100,ErrorMessage ="Amount must be between 1 : 100 ")]
        public int Count { get; set; }

        public string ApplicationUserId { get; set; }
        
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
