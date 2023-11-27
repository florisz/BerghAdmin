using BerghAdmin.Data.Kentaa;
using BerghAdmin.Services.Bihz;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace BerghAdmin;

public class EndpointHandler
{
    public IOptions<ApiConfiguration> Settings { get; }

    public EndpointHandler(IOptions<ApiConfiguration> settings)
    {
        Settings = settings;
    }

    public void CreateEndpoints(WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        app.MapPost("/actions",
            (BihzActie action, IBihzActieService service, HttpRequest req) => HandleNewActionAsync(action, service, req))
            .AllowAnonymous();
        app.MapPost("/donations",
            (BihzDonatie donation, IBihzDonatieService service, HttpRequest req) => HandleNewDonatie(donation, service, req))
            .AllowAnonymous();
        app.MapPost("/projects",
            (BihzProject project, IBihzProjectService service, HttpRequest req) => HandleNewProject(project, service, req))
            .AllowAnonymous();
        app.MapPost("/users",
            (BihzUser user, IBihzUserService service, HttpRequest req) => HandleNewUser(user, service, req))
            .AllowAnonymous();
    }

    public async Task<IResult> HandleNewActionAsync(BihzActie action, IBihzActieService service, HttpRequest req)
    {
        if (ApiKeyMissing(req))
            return Results.Unauthorized();

        await service.AddAsync(action);
        
        return Results.Ok("Ik heb n Action toegevoegd");
    }

    public IResult HandleNewDonatie(BihzDonatie donation, IBihzDonatieService service, HttpRequest req)
    {
        if (ApiKeyMissing(req))
            return Results.Unauthorized();

        service.AddAsync(donation);
        return Results.Ok("Ik heb n Donation toegevoegd");
    }

    public IResult HandleNewProject(BihzProject project, IBihzProjectService service, HttpRequest req)
    {
        if (ApiKeyMissing(req))
            return Results.Unauthorized();

        service.AddAsync(project);
        return Results.Ok("Ik heb n Project toegevoegd");
    }

    public IResult HandleNewUser(BihzUser user, IBihzUserService service, HttpRequest req)
    {
        if (ApiKeyMissing(req))
            return Results.Unauthorized();

        service.AddAsync(user);
        return Results.Ok("Ik heb n User toegevoegd");
    }

    bool ApiKeyMissing(HttpRequest req)
    {
        const string APIKEYNAME = "api-key";
        var headers = req.Headers;
        if (!headers.ContainsKey(APIKEYNAME))
            return true;

        var apiKey = headers[APIKEYNAME];
        return apiKey != this.Settings.Value.Key;
    }
}
