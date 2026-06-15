using BackOffice.Api.Models;
using BackOffice.Api.Services;
using Xunit;

namespace BackOffice.Tests
{
    // Characterizes the CURRENT pricing behaviour so the AI has a definition of done
    // when it touches discount rules.
    public class PricingTests
    {
        private readonly PricingService _pricing = new PricingService();

        [Fact]
        public void Subtotal_is_sum_of_line_totals()
        {
            var lines = new List<OrderLine>
            {
                new OrderLine { UnitPrice = 10m, Quantity = 2 },
                new OrderLine { UnitPrice = 5m, Quantity = 3 }
            };
            Assert.Equal(35m, _pricing.ComputeSubtotal(lines));
        }

        [Theory]
        [InlineData("Gold", 100, 10)]
        [InlineData("Corporate", 100, 10)]
        [InlineData("Standard", 100, 0)]
        [InlineData(null, 100, 0)]
        public void Discount_depends_on_customer_type(string type, decimal subtotal, decimal expected)
        {
            Assert.Equal(expected, _pricing.ComputeDiscount(type, subtotal));
        }
    }
}
