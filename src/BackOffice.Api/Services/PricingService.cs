using BackOffice.Api.Models;

namespace BackOffice.Api.Services
{
    // Yet another copy of the discount rules (see PricingHelper and OrderService).
    public class PricingService
    {
        public decimal ComputeSubtotal(List<OrderLine> lines)
        {
            decimal subtotal = 0m;
            foreach (var l in lines)
            {
                subtotal += l.UnitPrice * l.Quantity;
            }
            return subtotal;
        }

        public decimal ComputeDiscount(string customerType, decimal subtotal)
        {
            decimal discount = 0m;
            // Duplicated branching: Gold and Corporate both happen to be 10% today.
            if (customerType == "Gold")
            {
                discount = subtotal * 0.10m;
            }
            else if (customerType == "Corporate")
            {
                discount = subtotal * 0.10m;
            }
            return discount;
        }
    }
}
