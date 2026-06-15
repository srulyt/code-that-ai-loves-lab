using BackOffice.Api.Dtos;
using BackOffice.Api.Services;

namespace BackOffice.Api.Controllers
{
    // Minimal API "controller". Endpoint handlers do validation and some business
    // logic inline before (and sometimes instead of) delegating to services.
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this WebApplication app)
        {
            var orderService = new OrderService();

            app.MapGet("/orders", () =>
            {
                var repo = new BackOffice.Api.Repositories.OrderRepository();
                return Results.Ok(repo.GetAll());
            });

            app.MapGet("/orders/{id}", (string id) =>
            {
                var repo = new BackOffice.Api.Repositories.OrderRepository();
                var order = repo.GetById(id);
                if (order == null) return Results.NotFound(new { error = "Order not found" });
                return Results.Ok(order);
            });

            app.MapPost("/orders", (PlaceOrderRequest request) =>
            {
                // Inline validation duplicated from OrderService.PlaceOrder.
                if (request == null || string.IsNullOrWhiteSpace(request.CustomerId))
                {
                    return Results.BadRequest(new { error = "CustomerId is required" });
                }
                if (request.Lines == null || request.Lines.Count == 0)
                {
                    return Results.BadRequest(new { error = "At least one line is required" });
                }

                try
                {
                    var order = orderService.PlaceOrder(request);

                    // Re-render the invoice line here using the helper (a 4th place that
                    // knows about invoice formats).
                    var invoiceText = BackOffice.Api.Helpers.InvoiceHelper.Render(
                        order.InvoiceFormat, order.InvoiceNumber, order.Total);

                    return Results.Ok(new
                    {
                        order.Id,
                        order.Total,
                        order.ShippingCost,
                        order.DiscountAmount,
                        invoice = invoiceText
                    });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });
        }
    }
}
