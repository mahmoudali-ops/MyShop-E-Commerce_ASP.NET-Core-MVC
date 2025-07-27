using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Entities.Models;
using MyShop.Web.ViewModels;

namespace MyShop.Entities.Repositories
{
    public interface IShopingCartRepository : IGenericRepository<ShopingCart>
    {
        int IncreaseCount(ShopingCart shopingCart, int count);
        int DecreaseCount(ShopingCart shopingCart, int count);
    }
}
