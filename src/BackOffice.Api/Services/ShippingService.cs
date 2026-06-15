using BackOffice.Api.Models;

namespace BackOffice.Api.Services
{
    // Third copy of the shipping rules (see ShippingHelper and OrderService.PlaceOrder).
    public class ShippingService
    {
        public decimal ComputeShipping(string shippingMethod, string customerType, decimal subtotal, List<OrderLine> lines)
        {
            bool hasFragile = false;
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Fragile)
                {
                    hasFragile = true;
                }
            }

            decimal cost;
            if (shippingMethod == "Pickup")
            {
                cost = 0m;
            }
            else if (shippingMethod == "Express")
            {
                cost = 19.95m;
                // Gold free express over 150 (threshold hardcoded again).
                if (customerType == "Gold" && subtotal > 150m)
                {
                    cost = 0m;
                }
            }
            else
            {
                cost = 7.95m;
            }

            if (hasFragile && cost > 0m)
            {
                cost += 4.50m;
            }
            return cost;
        }
    }
}
