namespace BackOffice.Api.Models
{
    public class OrderLine
    {
        public string ProductId { get; set; }
        // Copied from Product at order time. Now we have two sources of truth.
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        // Copied from Product. Shipping rules read this instead of the product catalog.
        public bool Fragile { get; set; }
    }
}
