using Azure.Identity;

using BerghAdmin.ApplicationServices.Mail;

using BerghMonitor.Web;

using HealthChecks.UI.Core;

using Mailjet.Client;

using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
if (string.IsNullOrEmpty(env))
    throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT not set");

builder.Configuration
    .AddUserSecrets<SendMailService>()
    .AddAzureKeyVault(
        new Uri($"https://bergh-{env}-keyvault.vault.azure.net"),
        new DefaultAzureCredential()
    );

builder.Services
    .AddTransient<ISendMailService, SendMailService>()
    .AddHttpClient<IMailjetClient, MailjetClient>(client =>
    {
        //set BaseAddress, MediaType, UserAgent
        client.SetDefaultSettings();

        string apiKey = builder.Configuration["MailJetConfiguration:ApiKey"];
        string apiSecret = builder.Configuration["MailJetConfiguration:ApiSecret"];
        client.UseBasicAuthentication(apiKey, apiSecret);
    });

builder.Services
    .AddHealthChecksUI(setup =>
    {
        var webhookSettings = builder.Configuration.GetSection("HealthChecksUI:Webhooks");
        setup.AddWebhookNotification(
            "mail-admin",
            webhookSettings["Uri"],
            webhookSettings["Payload"],
            webhookSettings["RestoredPayload"],
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

app.MapPost("/mail", ([FromBody] MailRequest payload) => HandleSendMail(payload))
    .AllowAnonymous();

async Task<object> HandleSendMail(MailRequest payload)
{
    var sender = app.Services.GetRequiredService<ISendMailService>();
    await sender.SendMail( payload.To, "sysadmin@berghintzadel.nl", payload.Subject, payload.Body );

    return new OkObjectResult("Sent");
}

app.UseHttpsRedirection();
app.Run();