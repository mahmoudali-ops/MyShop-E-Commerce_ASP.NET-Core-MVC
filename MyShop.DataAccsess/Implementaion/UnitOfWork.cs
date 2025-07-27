using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Repositories;

namespace MyShop.DataAccsess.Implementaion
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;
        public ICategoryRepository category { get; private set; }
        public IProductRepository product { get; private set; }

        public IShopingCartRepository shopingcart { get; private set; }

        public IOrderHeaderRepository orderheader { get; private set; }

        public IOrderDetailsRepository orderdetails { get; private set; }
        public IAppUserRepository appUser { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            category = new CategoryRepository(context);
            product= new ProductRepository(context);
            shopingcart = new ShopingCartRepository(context);
            orderheader = new OrderHeaderRepository(context);
            orderdetails = new OrderDetailsRepository(context);
            appUser = new AppUserRepository(context);
        }

        public int Complete()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
             _context.Dispose();
        }
    }
}
