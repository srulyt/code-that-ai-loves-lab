using BackOffice.Api.Services;

namespace BackOffice.Api.Controllers
{
    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this WebApplication app)
        {
            var reportService = new ReportService();

            app.MapGet("/reports/summary", () =>
            {
                return Results.Ok(reportService.BuildSummary());
            });
        }
    }
}
