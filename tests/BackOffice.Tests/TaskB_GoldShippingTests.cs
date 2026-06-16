using BackOffice.Api.Models;
using BackOffice.Api.Services;
using Xunit;

namespace BackOffice.Tests
{
    // TASK B - "Gold free express only when basket > threshold AND >= 1 non-fragile item".
    //
    // The first two tests pin the CURRENT behaviour (used as regression guards).
    // The [Fact(Skip=...)] tests describe the TARGET behaviour for Task B. Students
    // (or the AI) remove the Skip and make them pass when implementing the change.
    public class TaskB_GoldShippingTests
    {
        private readonly ShippingService _shipping = new ShippingService();

        private static List<OrderLine> Basket(params bool[] fragileFlags)
        {
            var lines = new List<OrderLine>();
            foreach (var f in fragileFlags)
            {
                lines.Add(new OrderLine { UnitPrice = 100m, Quantity = 1, Fragile = f });
            }
            return lines;
        }

        // ---- current behaviour (regression guards) ----

        [Fact]
        public void Current_gold_free_express_over_threshold()
        {
            // Today: only the total matters, fragility is ignored for the free-express decision.
            Assert.Equal(0m, _shipping.ComputeShipping("Express", "Gold", 200m, Basket(false)));
        }

        [Fact]
        public void Current_gold_pays_express_under_threshold()
        {
            Assert.Equal(19.95m, _shipping.ComputeShipping("Express", "Gold", 100m, Basket(false)));
        }

        // ---- target behaviour for Task B ----

        [Fact(Skip = "Task B: enable when the non-fragile condition is implemented.")]
        public void Target_gold_free_express_requires_a_non_fragile_item()
        {
            // Over threshold but every item is fragile -> should NOT get free express.
            var allFragile = Basket(true, true);
            Assert.Equal(19.95m + 4.50m, _shipping.ComputeShipping("Express", "Gold", 300m, allFragile));
        }

        [Fact(Skip = "Task B: enable when the non-fragile condition is implemented.")]
        public void Target_gold_free_express_when_over_threshold_and_has_non_fragile()
        {
            var mixed = Basket(true, false);
            Assert.Equal(0m, _shipping.ComputeShipping("Express", "Gold", 300m, mixed));
        }

        [Fact(Skip = "Task B: threshold must be configurable, not hardcoded to 150.")]
        public void Target_threshold_is_configurable()
        {
            // DEFINITION OF DONE:
            //   Today the gold free-express threshold is the literal 150 baked into ShippingService
            //   (and copied elsewhere). After the refactor it must come from configuration/policy
            //   (e.g. AppConfig.GoldFreeExpressThreshold), so the boundary moves when config changes.
            //
            //   Arrange: read the configured threshold instead of assuming 150.
            //     decimal threshold = AppConfig.GoldFreeExpressThreshold;   // <-- introduced in Lab 3/4
            //     var nonFragile = Basket(false);
            //   Act / Assert: just ABOVE the configured threshold -> free express; AT/below -> pays express.
            //     Assert.Equal(0m,     _shipping.ComputeShipping("Express", "Gold", threshold + 1m, nonFragile));
            //     Assert.Equal(19.95m, _shipping.ComputeShipping("Express", "Gold", threshold,      nonFragile));
            //
            //   Note: on the messy baseline there is no AppConfig.GoldFreeExpressThreshold yet, so this
            //   stays skipped until the rule is single-homed and the threshold is read from config.
            Assert.True(false, "Implement per the pseudo-code above once the threshold is read from config.");
        }
    }
}
