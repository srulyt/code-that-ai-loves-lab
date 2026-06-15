namespace BackOffice.Api.Models
{
    public class Return
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerType { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public bool Fragile { get; set; }
        // "Requested" / "Approved" / "Rejected".
        public string Status { get; set; }
        public decimal RefundAmount { get; set; }
        public string CreatedUtc { get; set; }
    }
}
