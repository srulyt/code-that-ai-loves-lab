using BackOffice.Api.Models;
using BackOffice.Api.Services;
using Xunit;

namespace BackOffice.Tests
{
    public class ReturnTests
    {
        private readonly ReturnService _returns = new ReturnService();

        private static Order OrderCreatedDaysAgo(int days, string customerType, string productName = "Wireless Mouse")
        {
            return new Order
            {
                Id = "O-TEST",
                CustomerId = "C-TEST",
                CustomerType = customerType,
                CreatedUtc = DateTime.UtcNow.AddDays(-days).ToString("o"),
                Lines = new List<OrderLine>
                {
                    new OrderLine { ProductId = "P-4", ProductName = productName, UnitPrice = 29.95m, Quantity = 1 }
                }
            };
        }

        [Fact]
        public void Eligible_within_standard_window()
        {
            var order = OrderCreatedDaysAgo(10, "Standard");
            Assert.True(_returns.IsEligible(order, "P-4", out _));
        }

        [Fact]
        public void Not_eligible_after_standard_window()
        {
            var order = OrderCreatedDaysAgo(31, "Standard");
            Assert.False(_returns.IsEligible(order, "P-4", out var reason));
            Assert.Equal("Outside return window", reason);
        }

        [Fact]
        public void Corporate_has_longer_window()
        {
            var order = OrderCreatedDaysAgo(45, "Corporate");
            Assert.True(_returns.IsEligible(order, "P-4", out _));
        }

        [Fact]
        public void Gift_cards_are_not_returnable()
        {
            var order = OrderCreatedDaysAgo(1, "Standard", productName: "Gift Card");
            Assert.False(_returns.IsEligible(order, "P-4", out var reason));
            Assert.Equal("Gift cards are not returnable", reason);
        }

        [Fact]
        public void Null_order_is_not_eligible()
        {
            Assert.False(_returns.IsEligible(null, "P-4", out var reason));
            Assert.Equal("Order not found", reason);
        }
    }
}
