namespace BackOffice.Api.Models
{
    // Used as BOTH the persisted record and the in-flight domain object.
    // There is no separation between storage shape and business shape (DTO-as-domain smell).
    public class Order
    {
        public string Id { get; set; }
        public string CustomerId { get; set; }
        // Customer type is denormalised onto the order so callers don't have to load
        // the customer again. When a customer changes tier, this silently goes stale.
        public string CustomerType { get; set; }
        // "Standard" / "Marketplace" / "StorePickup" (magic strings drive switch logic everywhere).
        public string Type { get; set; }
        // "Standard" / "Express" / "Pickup".
        public string ShippingMethod { get; set; }
        public List<OrderLine> Lines { get; set; } = new List<OrderLine>();

        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }

        public string InvoiceNumber { get; set; }
        // "ConsumerStandard" / "CorporateNet30" (selected by duplicated logic in several files).
        public string InvoiceFormat { get; set; }

        public string Status { get; set; }
        public string CreatedUtc { get; set; }
    }
}
