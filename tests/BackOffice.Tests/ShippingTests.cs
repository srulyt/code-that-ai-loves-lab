using BackOffice.Api.Models;
using BackOffice.Api.Services;
using Xunit;

namespace BackOffice.Tests
{
    public class ShippingTests
    {
        private readonly ShippingService _shipping = new ShippingService();

        private static List<OrderLine> Lines(bool fragile = false) => new List<OrderLine>
        {
            new OrderLine { UnitPrice = 10m, Quantity = 1, Fragile = fragile }
        };

        [Fact]
        public void Pickup_is_free()
        {
            Assert.Equal(0m, _shipping.ComputeShipping("Pickup", "Standard", 200m, Lines()));
        }

        [Fact]
        public void Standard_is_flat_rate()
        {
            Assert.Equal(7.95m, _shipping.ComputeShipping("Standard", "Standard", 50m, Lines()));
        }

        [Fact]
        public void Express_is_flat_rate_for_non_gold()
        {
            Assert.Equal(19.95m, _shipping.ComputeShipping("Express", "Standard", 500m, Lines()));
        }

        [Fact]
        public void Gold_gets_free_express_over_threshold()
        {
            Assert.Equal(0m, _shipping.ComputeShipping("Express", "Gold", 151m, Lines()));
        }

        [Fact]
        public void Gold_pays_express_at_or_below_threshold()
        {
            Assert.Equal(19.95m, _shipping.ComputeShipping("Express", "Gold", 150m, Lines()));
        }

        [Fact]
        public void Fragile_adds_surcharge_when_shipping_is_charged()
        {
            Assert.Equal(7.95m + 4.50m, _shipping.ComputeShipping("Standard", "Standard", 50m, Lines(fragile: true)));
        }

        [Fact]
        public void Fragile_surcharge_does_not_apply_to_free_shipping()
        {
            Assert.Equal(0m, _shipping.ComputeShipping("Pickup", "Standard", 50m, Lines(fragile: true)));
        }
    }
}
