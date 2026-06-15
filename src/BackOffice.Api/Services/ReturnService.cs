using System.Globalization;
using BackOffice.Api.Dtos;
using BackOffice.Api.Models;
using BackOffice.Api.Repositories;

namespace BackOffice.Api.Services
{
    public class ReturnService
    {
        private readonly OrderRepository _orders = new OrderRepository();
        private readonly ReturnRepository _returns = new ReturnRepository();

        // Return eligibility lives here AND is re-implemented in ReturnEndpoints.
        public bool IsEligible(Order order, string productId, out string reason)
        {
            reason = "";
            if (order == null)
            {
                reason = "Order not found";
                return false;
            }

            // Window depends on customer type. Magic numbers duplicated from AppConfig.
            int windowDays = 30;
            if (order.CustomerType == "Corporate")
            {
                windowDays = 60;
            }

            DateTime created;
            if (!DateTime.TryParse(order.CreatedUtc, CultureInfo.InvariantCulture,
                    DateTimeStyles.AdjustToUniversal, out created))
            {
                created = DateTime.UtcNow;
            }
            if ((DateTime.UtcNow - created).TotalDays > windowDays)
            {
                reason = "Outside return window";
                return false;
            }

            // Gift cards are never returnable. Identified by product name string match.
            foreach (var l in order.Lines)
            {
                if (l.ProductId == productId && l.ProductName != null &&
                    l.ProductName.ToLower().Contains("gift card"))
                {
                    reason = "Gift cards are not returnable";
                    return false;
                }
            }

            return true;
        }

        public Return CreateReturn(CreateReturnRequest request)
        {
            var order = _orders.GetById(request.OrderId);
            string reason;
            if (!IsEligible(order, request.ProductId, out reason))
            {
                throw new InvalidOperationException("Return not eligible: " + reason);
            }

            // Find the line to compute the refund and copy the fragile flag (again).
            decimal unitPrice = 0m;
            bool fragile = false;
            foreach (var l in order.Lines)
            {
                if (l.ProductId == request.ProductId)
                {
                    unitPrice = l.UnitPrice;
                    fragile = l.Fragile;
                }
            }

            var ret = new Return
            {
                Id = "R-" + (9000 + _returns.GetAll().Count + 1),
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CustomerType = order.CustomerType,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Reason = request.Reason,
                Fragile = fragile,
                Status = "Approved",
                RefundAmount = unitPrice * request.Quantity,
                CreatedUtc = DateTime.UtcNow.ToString("o")
            };
            _returns.Add(ret);
            return ret;
        }
    }
}
