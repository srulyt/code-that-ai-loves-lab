namespace BackOffice.Api.Dtos
{
    // Request body for creating a return.
    public class CreateReturnRequest
    {
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}
