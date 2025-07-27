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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void update(Category category)
        {
            var UpdatedCategory = _context.categories.FirstOrDefault(e=>e.ID==category.ID);
            if (UpdatedCategory != null) 
            {
                UpdatedCategory.Name = category.Name;
                UpdatedCategory.Description = category.Description; 
                UpdatedCategory.CreatedTime = DateTime.Now;
            }
        }
    }
}
