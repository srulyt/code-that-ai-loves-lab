using BackOffice.Api.Repositories;

namespace BackOffice.Api.Services
{
    // LARGE ORCHESTRATION METHOD #2 lives in BuildSummary().
    public class ReportService
    {
        private readonly OrderRepository _orders = new OrderRepository();
        private readonly ReturnRepository _returns = new ReturnRepository();
        private readonly CustomerRepository _customers = new CustomerRepository();

        public Dictionary<string, object> BuildSummary()
        {
            var orders = _orders.GetAll();
            var returns = _returns.GetAll();
            var customers = _customers.GetAll();

            decimal grossRevenue = 0m;
            decimal totalDiscounts = 0m;
            decimal totalShipping = 0m;
            decimal refunds = 0m;

            int standardCount = 0;
            int marketplaceCount = 0;
            int storePickupCount = 0;

            int goldOrders = 0;
            int corporateOrders = 0;
            int standardCustomerOrders = 0;

            int consumerInvoices = 0;
            int corporateInvoices = 0;

            int freeExpressGiven = 0;
            int fragileOrders = 0;

            // One giant pass with a forest of if/switch branches by type and tier.
            foreach (var o in orders)
            {
                grossRevenue += o.Total;
                totalDiscounts += o.DiscountAmount;
                totalShipping += o.ShippingCost;

                switch (o.Type)
                {
                    case "Standard":
                        standardCount++;
                        break;
                    case "Marketplace":
                        marketplaceCount++;
                        break;
                    case "StorePickup":
                        storePickupCount++;
                        break;
                }

                if (o.CustomerType == "Gold")
                {
                    goldOrders++;
                }
                else if (o.CustomerType == "Corporate")
                {
                    corporateOrders++;
                }
                else
                {
                    standardCustomerOrders++;
                }

                if (o.InvoiceFormat == "CorporateNet30")
                {
                    corporateInvoices++;
                }
                else
                {
                    consumerInvoices++;
                }

                // Re-derive "was free express given?" by re-checking the rule yet again.
                if (o.ShippingMethod == "Express" && o.CustomerType == "Gold" &&
                    o.Subtotal > 150m && o.ShippingCost == 0m)
                {
                    freeExpressGiven++;
                }

                bool hasFragile = false;
                foreach (var l in o.Lines)
                {
                    if (l.Fragile) hasFragile = true;
                }
                if (hasFragile) fragileOrders++;
            }

            foreach (var r in returns)
            {
                refunds += r.RefundAmount;
            }

            var summary = new Dictionary<string, object>
            {
                ["customers"] = customers.Count,
                ["orders"] = orders.Count,
                ["returns"] = returns.Count,
                ["grossRevenue"] = grossRevenue,
                ["totalDiscounts"] = totalDiscounts,
                ["totalShipping"] = totalShipping,
                ["refunds"] = refunds,
                ["netRevenue"] = grossRevenue - refunds,
                ["ordersByType"] = new Dictionary<string, int>
                {
                    ["Standard"] = standardCount,
                    ["Marketplace"] = marketplaceCount,
                    ["StorePickup"] = storePickupCount
                },
                ["ordersByCustomerTier"] = new Dictionary<string, int>
                {
                    ["Gold"] = goldOrders,
                    ["Corporate"] = corporateOrders,
                    ["Standard"] = standardCustomerOrders
                },
                ["invoicesByFormat"] = new Dictionary<string, int>
                {
                    ["ConsumerStandard"] = consumerInvoices,
                    ["CorporateNet30"] = corporateInvoices
                },
                ["freeExpressGiven"] = freeExpressGiven,
                ["fragileOrders"] = fragileOrders
            };
            return summary;
        }
    }
}
