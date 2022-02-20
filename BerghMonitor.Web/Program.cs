using BerghMonitor.Web;

using HealthChecks.UI.Core;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services
        .AddHealthChecksUI(setup =>
        {
            setup.AddWebhookNotification("mail-admin",
                uri: "/mail", 
                payload: "{ \"body\": \"Webhook report for [[LIVENESS]]: [[FAILURE]] - Description: [[DESCRIPTIONS]]\"}",
                restorePayload: "{ \"subject\": \"[[LIVENESS]] is back to life\"}",
                shouldNotifyFunc: report => DateTime.UtcNow.Hour >= 8 && DateTime.UtcNow.Hour <= 23,
                customMessageFunc: (report) =>
                {
                    var failing = report.Entries.Where(e => e.Value.Status == UIHealthStatus.Unhealthy);
                    return $"{failing.Count()} healthchecks are failing";
                },
                customDescriptionFunc: report =>
                {
                    var failing = report.Entries.Where(e => e.Value.Status == UIHealthStatus.Unhealthy);
                    return $"HealthChecks with names {string.Join("/", failing.Select(f => f.Key))} are failing";
                });
        })
        .AddInMemoryStorage();

var app = builder.Build();
app
    .UseRouting()
    .UseEndpoints(config =>
    {
        config.MapHealthChecksUI(s => s.UseRelativeWebhookPath = true);
    });

//app.MapGet("mail", (string payload) => "");
app.MapPost("/mail", ([FromBody]MailRequest payload) => HandleSendMail(payload))
    .AllowAnonymous();

object HandleSendMail(MailRequest payload)
{
    return new OkObjectResult("Sent");
}

app.UseHttpsRedirection();
app.Run();