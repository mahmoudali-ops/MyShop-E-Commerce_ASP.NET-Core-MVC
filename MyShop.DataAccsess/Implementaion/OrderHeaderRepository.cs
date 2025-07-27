using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccsess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;

namespace MyShop.DataAccsess.Implementaion
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        public readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void update(OrderHeader orderHeader)
        {
            _context.orderHeaders.Update(orderHeader);
        }

        public void updateStatus(int Id, string OrderStatus, string PaymentStatus)
        {
            var Order = _context.orderHeaders.Find(Id);

            if (Order != null) 
            {
                Order.OrderStatus = OrderStatus;
                if (Order.PaymentStatus != null) 
                {
                    Order.PaymentStatus = PaymentStatus;
                }
            }
        }
    }
}
