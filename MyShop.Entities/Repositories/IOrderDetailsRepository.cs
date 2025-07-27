using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        void update(OrderDetails orderDetails);

    }
}
