using BackOffice.Api.Helpers;
using BackOffice.Api.Services;
using Xunit;

namespace BackOffice.Tests
{
    public class InvoiceTests
    {
        private readonly InvoiceService _invoices = new InvoiceService();

        [Theory]
        [InlineData("Corporate", "Standard", "CorporateNet30")]
        [InlineData("Gold", "Standard", "ConsumerStandard")]
        [InlineData("Standard", "Marketplace", "ConsumerStandard")]
        public void Format_depends_on_customer_type(string customerType, string orderType, string expected)
        {
            Assert.Equal(expected, _invoices.SelectFormat(customerType, orderType));
        }

        [Fact]
        public void Corporate_invoice_renders_net30_terms()
        {
            var text = InvoiceHelper.Render("CorporateNet30", "INV-1", 100m);
            Assert.Contains("NET 30", text);
        }

        [Fact]
        public void Consumer_invoice_renders_paid()
        {
            var text = InvoiceHelper.Render("ConsumerStandard", "INV-1", 100m);
            Assert.Contains("Paid", text);
        }
    }
}
