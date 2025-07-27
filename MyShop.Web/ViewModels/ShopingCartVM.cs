using MyShop.Entities.Models;

namespace MyShop.Web.ViewModels
{
    public class ShopingCartVM
    {
        public OrderHeader OrderHeader { get; set; } = new();

        public IEnumerable<ShopingCart> CartsList { get; set; }

        public decimal TotalCost { get; set; }
    }
}
