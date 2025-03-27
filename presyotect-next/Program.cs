using Cron.Core;
using Cron.Core.Enums;
using Hangfire;
using Presyotect.Features.Monitoring.Services;
using Presyotect.Utilities;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddCoreServices();
builder.AddAuthServices();
builder.AddDataServices();

var app = builder.Build();

app.UseCors(
    options =>
    {
        options.SetIsOriginAllowed(_ => true)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });

app.UseAuthentication();
app.UseAuthorization();

#if DEBUG
app.UseHangfireDashboard();
#endif

app.UseSwagger(options => { options.RouteTemplate = "/openapi/{documentName}.json"; });
app.MapScalarApiReference(endpointPrefix: "_api_docs", options => { options.Theme = ScalarTheme.Kepler; });
#if DEBUG
app.MapGet("/", (context) =>
{
    context.Response.Redirect("/_api_docs/");
    return Task.CompletedTask;
});
#else
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
#endif

app.MapEndpoints();
app.MapGet("/_api/check", () => true);

app.Lifetime.ApplicationStarted.Register(() =>
{
    _ = app.Services.GetRequiredService<MonitoringScheduler>();
});

app.Run();