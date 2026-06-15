namespace BackOffice.Api.Models
{
    // Anemic POCO. No behaviour lives here in the baseline; rules are scattered
    // across services and helpers instead.
    public class Customer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // Business concept encoded as a magic string ("Standard" / "Gold" / "Corporate").
        public string Type { get; set; }
        public string Email { get; set; }
    }
}
