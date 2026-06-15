using BackOffice.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5080");

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    service = "Retail Order Processing & Returns Back Office",
    endpoints = new[]
    {
        "GET  /customers",
        "GET  /products",
        "GET  /orders",
        "GET  /orders/{id}",
        "POST /orders",
        "GET  /returns",
        "POST /returns",
        "GET  /reports/summary"
    }
}));

// A couple of read endpoints are wired up inline here instead of in a handler class,
// because "it was just one line at the time".
app.MapGet("/customers", () =>
{
    var repo = new BackOffice.Api.Repositories.CustomerRepository();
    return Results.Ok(repo.GetAll());
});

app.MapGet("/products", () =>
{
    var repo = new BackOffice.Api.Repositories.ProductRepository();
    return Results.Ok(repo.GetAll());
});

app.MapOrderEndpoints();
app.MapReturnEndpoints();
app.MapReportEndpoints();

app.Run();

// Exposed so the test project (on the baseline-with-tests branch) can reference the app.
public partial class Program { }
