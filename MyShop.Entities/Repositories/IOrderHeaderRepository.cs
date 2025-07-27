using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Entities.Models;

namespace MyShop.Entities.Repositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void update(OrderHeader orderHeader);

        void updateStatus(int Id, string? OrderStatus, string? PaymentStatus);

    }
}
