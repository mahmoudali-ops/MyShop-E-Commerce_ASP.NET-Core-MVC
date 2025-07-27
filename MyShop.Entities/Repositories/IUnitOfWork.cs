using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable 
    {
        ICategoryRepository category { get; }
        IProductRepository product { get; }
        IShopingCartRepository shopingcart { get; }

        IOrderHeaderRepository orderheader { get; }
         IOrderDetailsRepository orderdetails { get; }

        IAppUserRepository appUser { get; }
        int Complete();
    }
}
