using Xunit;

namespace BackOffice.Tests
{
    // TASK A - "Corporate Gift Order".
    //
    // None of this exists in the baseline yet, so every test is skipped. They form a
    // ready-made definition of done: students (or the AI) implement the feature and
    // remove the Skip markers one by one until they pass.
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
            Assert.True(false, "A gift order should accept more than one GiftRecipient.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_carries_a_gift_message()
        {
            Assert.True(false, "A gift order should persist a gift message.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_corporate_agreement_pricing_not_retail_promotions()
        {
            Assert.True(false, "Pricing should follow the corporate agreement, not retail discounts.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_a_distinct_invoice_format()
        {
            Assert.True(false, "Gift orders should not render as ConsumerStandard or plain CorporateNet30.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_uses_a_distinct_return_policy()
        {
            Assert.True(false, "Gift order returns should differ from standard retail returns.");
        }

        [Fact(Skip = "Task A: implement Corporate Gift Order, then enable.")]
        public void Gift_order_appears_in_the_report_summary()
        {
            Assert.True(false, "BuildSummary should count gift orders.");
        }
    }
}
