namespace BackOffice.Api.Helpers
{
    // Invoice format selection, duplicated from InvoiceService.
    public static class InvoiceHelper
    {
        public static string Format(string customerType, string orderType)
        {
            // Corporate customers get net-30 invoices; everyone else gets the consumer format.
            if (customerType == "Corporate")
            {
                return "CorporateNet30";
            }
            return "ConsumerStandard";
        }

        public static string Render(string format, string invoiceNumber, decimal total)
        {
            if (format == "CorporateNet30")
            {
                return $"INVOICE {invoiceNumber} | TERMS: NET 30 | AMOUNT DUE: {total:C}";
            }
            return $"Invoice {invoiceNumber} | Paid: {total:C}";
        }
    }
}
