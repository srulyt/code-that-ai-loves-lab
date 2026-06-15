using BackOffice.Api.Models;

namespace BackOffice.Api.Helpers
{
    // Shipping math, duplicated from ShippingService and OrderService.PlaceOrder.
    public static class ShippingHelper
    {
        public static decimal Shipping(string shippingMethod, string customerType, decimal subtotal, List<OrderLine> lines)
        {
            // Detect fragile items (this exact loop is repeated in several files).
            bool hasFragile = false;
            foreach (var l in lines)
            {
                if (l.Fragile)
                {
                    hasFragile = true;
                }
            }

            decimal cost = 0m;
            switch (shippingMethod)
            {
                case "Pickup":
                    cost = 0m;
                    break;
                case "Express":
                    cost = 19.95m;
                    // Gold gets free express over 150 (threshold hardcoded here).
                    if (customerType == "Gold" && subtotal > 150m)
                    {
                        cost = 0m;
                    }
                    break;
                case "Standard":
                default:
                    cost = 7.95m;
                    break;
            }

            if (hasFragile && cost > 0m)
            {
                cost = cost + 4.50m; // fragile surcharge
            }
            return cost;
        }
    }
}
