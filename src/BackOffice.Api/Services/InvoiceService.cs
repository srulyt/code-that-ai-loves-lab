using BackOffice.Api.Models;

namespace BackOffice.Api.Services
{
    public class InvoiceService
    {
        // Duplicate of InvoiceHelper.Format.
        public string SelectFormat(string customerType, string orderType)
        {
            if (customerType == "Corporate")
            {
                return "CorporateNet30";
            }
            else
            {
                return "ConsumerStandard";
            }
        }

        public string NextInvoiceNumber(int existingCount)
        {
            // "INV-" + 5000 + sequence. Collides if two orders are created concurrently.
            return "INV-" + (5000 + existingCount + 1).ToString();
        }

        public void AssignInvoice(Order order, int existingCount)
        {
            // Re-selects the format here as well (instead of trusting SelectFormat),
            // so the rule effectively exists twice inside this very class.
            if (order.CustomerType == "Corporate")
            {
                order.InvoiceFormat = "CorporateNet30";
            }
            else
            {
                order.InvoiceFormat = "ConsumerStandard";
            }
            order.InvoiceNumber = NextInvoiceNumber(existingCount);
        }
    }
}
