using MyShop.Entities.Models;
using System.Collections;

namespace MyShop.Web.ViewModels
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }

        public IEnumerable<OrderDetails> orderDetails { get; set; }
    }
}
