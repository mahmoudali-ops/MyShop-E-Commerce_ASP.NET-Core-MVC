using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Utilities
{
    public static class SD
    {
        // Roles

        public const string AdminRole = "Admin";
        public const string EditorRole = "Editor";
        public const string CustomerRole = "Customer";

        //order Status

        public const string Pending = "Pending";
        public const string Approve = "Approve";
        public const string Proccessing = "Proccessing";
        public const string Cancled = "Cancled";
        public const string Shipped = "Shipped";
        public const string Refund = "Refund";
        public const string Rejected = "Rejected";

        public const string SessionKey = "ShopingCartSession";


    }
}
