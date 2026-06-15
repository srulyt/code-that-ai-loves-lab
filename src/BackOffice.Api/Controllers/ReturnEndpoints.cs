using System.Globalization;
using BackOffice.Api.Dtos;
using BackOffice.Api.Repositories;
using BackOffice.Api.Services;

namespace BackOffice.Api.Controllers
{
    public static class ReturnEndpoints
    {
        public static void MapReturnEndpoints(this WebApplication app)
        {
            var returnService = new ReturnService();

            app.MapGet("/returns", () =>
            {
                var repo = new ReturnRepository();
                return Results.Ok(repo.GetAll());
            });

            app.MapPost("/returns", (CreateReturnRequest request) =>
            {
                var orders = new OrderRepository();
                var order = orders.GetById(request.OrderId);
                if (order == null)
                {
                    return Results.NotFound(new { error = "Order not found" });
                }

                // Return eligibility RE-IMPLEMENTED here, separately from ReturnService.IsEligible.
                // The window numbers are copied yet again.
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
                    return Results.BadRequest(new { error = "Outside return window" });
                }

                try
                {
                    var ret = returnService.CreateReturn(request);
                    return Results.Ok(ret);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });
        }
    }
}
