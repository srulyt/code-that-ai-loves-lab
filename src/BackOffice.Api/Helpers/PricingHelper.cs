namespace BackOffice.Api.Helpers
{
    // Discount math also lives here, duplicated from PricingService and OrderService.
    // If the Gold rate changes, you must remember to change it in all three places.
    public static class PricingHelper
    {
        public static decimal Discount(string customerType, decimal subtotal)
        {
            decimal discount = 0m;
            if (customerType == "Gold")
            {
                discount = subtotal * 0.10m; // 10%
            }
            else if (customerType == "Corporate")
            {
                discount = subtotal * 0.10m; // corporate agreement 10%
            }
            else
            {
                discount = 0m;
            }
            return discount;
        }
    }
}
