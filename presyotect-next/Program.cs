using Cron.Core;
using Cron.Core.Enums;
using Hangfire;
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
    var jobManager = app.Services.GetRequiredService<IRecurringJobManager>();
    
    var schedule = new CronBuilder()
        .Add(CronTimeSections.Minutes, 0)
        .Add(CronTimeSections.Hours, 7)
        .Add(CronDays.Monday);
    
    jobManager.AddOrUpdate(
        "run-every-thurs",
        () => Console.Write("its 4:12"),
        schedule.ToString, new RecurringJobOptions()
        {
            TimeZone = TimeZoneInfo.Local
        });
});

app.Run();