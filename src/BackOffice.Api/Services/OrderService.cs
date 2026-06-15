using BackOffice.Api.Dtos;
using BackOffice.Api.Helpers;
using BackOffice.Api.Models;
using BackOffice.Api.Repositories;

namespace BackOffice.Api.Services
{
    // Orchestration AND business logic AND mapping AND persistence all in one place.
    public class OrderService
    {
        private readonly CustomerRepository _customers = new CustomerRepository();
        private readonly ProductRepository _products = new ProductRepository();
        private readonly OrderRepository _orders = new OrderRepository();

        // LARGE ORCHESTRATION METHOD #1.
        // Validates, builds lines, prices, ships, invoices, persists and formats a
        // response - inline - re-deriving rules that also live in the services/helpers.
        public Order PlaceOrder(PlaceOrderRequest request)
        {
            // ---- validate ----
            if (request == null) throw new ArgumentException("Request is required");
            if (string.IsNullOrWhiteSpace(request.CustomerId)) throw new ArgumentException("CustomerId is required");
            if (request.Lines == null || request.Lines.Count == 0) throw new ArgumentException("At least one line is required");

            var customer = _customers.GetById(request.CustomerId);
            if (customer == null) throw new ArgumentException("Unknown customer " + request.CustomerId);

            // Order type defaults to Standard when not provided.
            var orderType = string.IsNullOrWhiteSpace(request.Type) ? "Standard" : request.Type;
            if (orderType != "Standard" && orderType != "Marketplace" && orderType != "StorePickup")
            {
                throw new ArgumentException("Unsupported order type " + orderType);
            }

            var shippingMethod = string.IsNullOrWhiteSpace(request.ShippingMethod) ? "Standard" : request.ShippingMethod;
            // StorePickup forces the Pickup shipping method.
            if (orderType == "StorePickup")
            {
                shippingMethod = "Pickup";
            }

            // ---- build lines + subtotal ----
            var lines = new List<OrderLine>();
            decimal subtotal = 0m;
            bool hasFragile = false;
            foreach (var reqLine in request.Lines)
            {
                var product = _products.GetById(reqLine.ProductId);
                if (product == null) throw new ArgumentException("Unknown product " + reqLine.ProductId);
                var line = new OrderLine
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = reqLine.Quantity,
                    Fragile = product.Fragile
                };
                subtotal += line.UnitPrice * line.Quantity;
                if (product.Fragile) hasFragile = true;
                lines.Add(line);
            }

            // ---- discount (inlined copy of the pricing rules) ----
            decimal discount = 0m;
            if (customer.Type == "Gold")
            {
                discount = subtotal * 0.10m;
            }
            else if (customer.Type == "Corporate")
            {
                discount = subtotal * 0.10m;
            }

            decimal discountedSubtotal = subtotal - discount;

            // ---- shipping (inlined copy of the shipping rules) ----
            decimal shippingCost;
            if (shippingMethod == "Pickup")
            {
                shippingCost = 0m;
            }
            else if (shippingMethod == "Express")
            {
                shippingCost = 19.95m;
                // Gold free express over 150 (threshold hardcoded a third time).
                if (customer.Type == "Gold" && subtotal > 150m)
                {
                    shippingCost = 0m;
                }
            }
            else
            {
                shippingCost = 7.95m;
            }
            if (hasFragile && shippingCost > 0m)
            {
                shippingCost += 4.50m;
            }

            // ---- invoice format (inlined copy of the invoice rules) ----
            string invoiceFormat;
            if (customer.Type == "Corporate")
            {
                invoiceFormat = "CorporateNet30";
            }
            else
            {
                invoiceFormat = "ConsumerStandard";
            }

            var existing = _orders.GetAll();
            var invoiceNumber = "INV-" + (5000 + existing.Count + 1);

            // ---- assemble + persist ----
            var order = new Order
            {
                Id = "O-" + (5000 + existing.Count + 1),
                CustomerId = customer.Id,
                CustomerType = customer.Type,
                Type = orderType,
                ShippingMethod = shippingMethod,
                Lines = lines,
                Subtotal = subtotal,
                DiscountAmount = discount,
                ShippingCost = shippingCost,
                Total = discountedSubtotal + shippingCost,
                InvoiceNumber = invoiceNumber,
                InvoiceFormat = invoiceFormat,
                Status = "Placed",
                CreatedUtc = DateTime.UtcNow.ToString("o")
            };
            _orders.Add(order);
            return order;
        }
    }
}
