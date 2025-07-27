using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Web.ViewModels;

namespace MyShop.DataAccsess.Implementaion
{
    public class ShopingCartRepository : GenericRepository<ShopingCart>, IShopingCartRepository
    {
        public readonly ApplicationDbContext _context;
        public ShopingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecreaseCount(ShopingCart shopingCart, int count)
        {
            shopingCart.Count -= count;
            return shopingCart.Count;
        }

        public int IncreaseCount(ShopingCart shopingCart, int count)
        {
            shopingCart.Count += count;
            return shopingCart.Count;
        }
    }
}
