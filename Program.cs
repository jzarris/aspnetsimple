using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

static object HttpBinBase(HttpContext ctx) => new
{
    args = ctx.Request.Query.ToDictionary(
        k => k.Key,
        v => v.Value.ToString()
    ),
    headers = ctx.Request.Headers.ToDictionary(
        h => h.Key,
        h => h.Value.ToString()
    ),
    method = ctx.Request.Method,
    origin = ctx.Connection.RemoteIpAddress?.ToString(),
    url = $"{ctx.Request.Scheme}://{ctx.Request.Host}{ctx.Request.Path}{ctx.Request.QueryString}"
};

app.MapGet("/", () => Results.Text($"""
Operating System: {RuntimeInformation.OSDescription}
.NET version: {Environment.Version}
Username: {Environment.UserName}
Date and Time: {DateTime.Now}
"""));

app.MapGet("/httpbin/get", (HttpContext ctx) =>
{
    return Results.Json(HttpBinBase(ctx));
});

app.Run();
