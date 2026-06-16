using Xunit;

namespace BackOffice.Tests
{
    // TASK A - "Corporate Gift Order".
    //
    // None of this exists in the baseline yet, so every test is skipped. Each test body carries
    // commented-out pseudo-code describing the intended arrange/act/assert (the definition of done)
    // plus a deliberately failing assertion. Implementing Task A means turning that pseudo-code into
    // real assertions against the new gift model and removing Skip — not just deleting the Skip marker.
    //
    // Business rules for a Corporate Gift Order:
    //   - can ship to multiple recipients
    //   - allows a gift message
    //   - uses corporate agreement pricing instead of retail promotions
    //   - invoice format differs from standard consumer orders
    //   - return policy differs from standard retail orders
    //   - is persisted and appears in summaries/reports
    public class TaskA_GiftOrderTests
    {
        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_supports_multiple_recipients()
        {
            // DEFINITION OF DONE (turn this pseudo-code into real code once the gift model exists):
            //
            //   Arrange: build a CorporateGift order for a corporate customer (seed C-1004) with TWO
            //            gift recipients and at least one line.
            //     var request = new PlaceOrderRequest {
            //         Type = "CorporateGift",
            //         CustomerId = "C-1004",
            //         ShippingMethod = "Standard",
            //         Lines = { new() { ProductId = "P-1", Quantity = 2 } },
            //         GiftRecipients = {                       // <-- new gift field you will add
            //             new() { Name = "Alice", Address = "1 Main St" },
            //             new() { Name = "Bob",   Address = "2 Oak Ave" },
            //         },
            //     };
            //   Act:     var order = orderService.PlaceOrder(request);
            //   Assert:  order persists BOTH recipients.
            //     Assert.Equal(2, order.GiftRecipients.Count);
            //
            // Until the gift model exists this stays skipped; do not just remove Skip.
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_carries_a_gift_message()
        {
            // DEFINITION OF DONE:
            //   Arrange: a CorporateGift order whose request carries a gift message.
            //     request.GiftMessage = "Season's greetings";   // <-- new gift field
            //   Act:     var order = orderService.PlaceOrder(request);
            //   Assert:  the message round-trips onto the persisted order.
            //     Assert.Equal("Season's greetings", order.GiftMessage);
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_corporate_agreement_pricing_not_retail_promotions()
        {
            // DEFINITION OF DONE:
            //   A gift order must be priced by the CORPORATE AGREEMENT, not retail promo discounts.
            //   Arrange: a CorporateGift order with a known subtotal (e.g. 1000m).
            //   Act:     place the order (or call the pricing path the gift order uses).
            //   Assert:  the discount equals the corporate-agreement amount, NOT the retail promo amount.
            //     // e.g. corporate agreement = 0% promo / negotiated terms:
            //     Assert.Equal(expectedCorporateAgreementDiscount, order.DiscountAmount);
            //     Assert.NotEqual(retailPromoDiscountForSameSubtotal, order.DiscountAmount);
            //   Note: today PricingService.ComputeDiscount keys only on customerType; Task A introduces
            //         order-type-aware (agreement) pricing for gift orders.
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_a_distinct_invoice_format()
        {
            // DEFINITION OF DONE (this rule already has a home: InvoiceService.SelectFormat(customerType, orderType)):
            //   Arrange: var invoices = new InvoiceService();
            //   Act:     var format = invoices.SelectFormat("Corporate", "CorporateGift");
            //   Assert:  gift orders get their OWN format, distinct from ConsumerStandard and plain CorporateNet30.
            //     Assert.Equal("CorporateGift", format);   // or whatever distinct format name you choose
            //     Assert.NotEqual("ConsumerStandard", format);
            //     Assert.NotEqual("CorporateNet30", format);
            //   Today SelectFormat ignores orderType and returns CorporateNet30/ConsumerStandard, so this fails.
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_a_distinct_return_policy()
        {
            // DEFINITION OF DONE (this rule already has a home: ReturnService.IsEligible(order, productId, out reason)):
            //   Arrange: var returns = new ReturnService();
            //            var giftOrder = new Order {
            //                Type = "CorporateGift", CustomerType = "Corporate",
            //                CreatedUtc = DateTime.UtcNow.AddDays(-80),     // beyond the 30/60-day standard windows
            //                Lines = { new OrderLine { ProductId = "P-1", ProductName = "..." } },
            //            };
            //   Act:     var ok = returns.IsEligible(giftOrder, "P-1", out var reason);
            //   Assert:  gift orders use a DISTINCT (longer, e.g. 90-day) return window, so this is still eligible
            //            where a standard/corporate order would not be.
            //     Assert.True(ok, reason);
            //   Today IsEligible only knows 30 (default) / 60 (Corporate) day windows keyed on CustomerType.
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_appears_in_the_report_summary()
        {
            // DEFINITION OF DONE:
            //   Arrange: place at least one CorporateGift order, then build the report summary.
            //     var summary = reportService.BuildSummary();
            //   Assert:  the summary counts gift orders in its by-type breakdown.
            //     Assert.True(summary.OrdersByType.ContainsKey("CorporateGift"));
            //     Assert.Equal(1, summary.OrdersByType["CorporateGift"]);
            //   Today BuildSummary has no CorporateGift category, so this fails.
            Assert.True(false, "Implement per the pseudo-code above, then replace this assertion.");
        }
    }
}
