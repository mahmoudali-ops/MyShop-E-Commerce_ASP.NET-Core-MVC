using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        void update(Category category);
    }
}
