using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyShop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DisplayName(" Product Image")]
        [ValidateNever]
        public string Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        
        [Required]
        [DisplayName(" Category")]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category category { get; set; }
    }
}
