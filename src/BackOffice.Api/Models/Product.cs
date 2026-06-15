namespace BackOffice.Api.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        // Drives shipping rules, but the flag is copied onto order lines too (implicit coupling).
        public bool Fragile { get; set; }
        public string Category { get; set; }
    }
}
