using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;

namespace MyShop.DataAccsess.Implementaion
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Product product)
        {
            var UpdatedProduct = _context.products.FirstOrDefault(e=>e.Id==product.Id);
            if (UpdatedProduct != null) 
            {
                UpdatedProduct.Name = product.Name;
                UpdatedProduct.Description = product.Description; 
                UpdatedProduct.Price = product.Price;
                UpdatedProduct.Img = product.Img;
                UpdatedProduct.CategoryId = product.CategoryId;
            }
        }
    }
}
