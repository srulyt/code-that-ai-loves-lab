namespace BackOffice.Api.Dtos
{
    // Incoming request body for placing an order. This DTO is passed deep into the
    // service layer and mutated along the way, so it doubles as a working domain object.
    public class PlaceOrderRequest
    {
        public string CustomerId { get; set; }
        public string Type { get; set; }            // Standard / Marketplace / StorePickup
        public string ShippingMethod { get; set; }  // Standard / Express / Pickup
        public List<PlaceOrderLine> Lines { get; set; } = new List<PlaceOrderLine>();
    }

    public class PlaceOrderLine
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
