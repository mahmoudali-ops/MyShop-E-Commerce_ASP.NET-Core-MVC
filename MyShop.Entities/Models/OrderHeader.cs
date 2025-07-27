using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Models
{
    public class OrderHeader
    {
        
        public int Id { get; set; }
        [ForeignKey("applicationUser")]
        public string ApplicatioUserId { get; set; }

        [ValidateNever]
        public ApplicationUser applicationUser  { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }

        public decimal TotalCost { get; set; }

        public string? PaymentStatus { get; set; }
        public string? OrderStatus { get; set; }

        public string? TrackingNumber { get; set; }

        public string? Carrier { get; set; }

        public DateTime PatmentDate { get; set; }

        // Stripe Properits
        public string? SessionId { get; set; }
        public string? PaymentInetId { get; set; }

        // User Details
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }

    }
}
