using BerghAdmin.ApplicationServices.Mail;
using BerghAdmin.Authorization;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Betalingen;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Seeding;
using HealthChecks.UI.Client;
using Mailjet.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using Syncfusion.Blazor;
using System.Text;

namespace BerghAdmin;

public class Program
{

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog((hc, lc) => lc
            .WriteTo.Console()
            .ReadFrom.Configuration(hc.Configuration)
        );

        Log.Information("Info: Hi there");
        Log.Warning("Warning: Hi there");
        Log.Debug("Debug: Hi there");

        RegisterAuthorization(builder.Services);
        RegisterServices(builder);

        var app = builder.Build();
        UseServices(app);
        CreateEndpoints(app);


        var seedDataService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedDataService>();
        seedDataService.SeedInitialData();
        var seedUsersService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedUsersService>();
        seedUsersService.SeedUsersData();

        app.Run();
    }

    private static void CreateEndpoints(WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        }).AllowAnonymous();
        app.MapPost("/actions",
            [Authorize(Policy = "ApiGebruiker")]
            (BihzActie action, IBihzActieService service, HttpRequest req) => HandleNewAction(action, service, req))
            .AllowAnonymous();
        app.MapPost("/donations",
            (BihzDonatie donation, IBihzDonatieService service) => HandleNewDonatie(donation, service))
            .AllowAnonymous();
        app.MapPost("/projects",
            (BihzProject project, IBihzProjectService service) => HandleNewProject(project, service))
            .AllowAnonymous();
        app.MapPost("/users",
            (BihzUser user, IBihzUserService service) => HandleNewUser(user, service))
            .AllowAnonymous();
    }

    static string GetDatabaseConnectionString(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        if (cs == null)
        {
            throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
        }
        return cs;
    }

    static void RegisterAuthorization(IServiceCollection services)
    {
        services
            .AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication("BasicAuthentication").AddScheme<ApiKeyOptions, ApiKeyAuthentication>("ApiKey", null);
        services.AddSingleton<IAuthorizationHandler, AdministratorPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerFietsersPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, ApiGebruikerPolicyHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdministrator", policy => policy.Requirements.Add(new IsAdministratorRequirement()));
            options.AddPolicy("BeheerFietsers", policy => policy.Requirements.Add(new IsFietsersBeheerderRequirement()));
            options.AddPolicy("ApiGebruiker", policy =>
            {
                policy.AuthenticationSchemes.Add("ApiKey");
                policy.Requirements.Add(new IsApiGebruikerRequirement());
            });
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

    }

    static void RegisterServices(WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddOptions();
        builder.Services.AddHttpClient();
        builder.Services.Configure<SeedSettings>(builder.Configuration.GetSection("Seeding"));
        builder.Services.AddScoped<IBetalingenService, BetalingenService>();
        builder.Services.AddScoped<IBetalingenImporterService, BetalingenImporterService>();
        builder.Services.AddScoped<IPersoonService, PersoonService>();
        builder.Services.AddScoped<IRolService, RolService>();
#if (DEBUG)
        builder.Services.AddTransient<ISeedDataService, DebugSeedDataService>();
        builder.Services.AddTransient<ISeedUsersService, DebugSeedUsersService>();
#elif (RELEASE)
        builder.Services.AddTransient<ISeedDataService, ReleaseSeedDataService>();
        builder.Services.AddTransient<ISeedUsersService, ReleaseSeedUsersService>();
#endif
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
        builder.Services.AddScoped<IDataImporterService, DataImporterService>();
        builder.Services.AddScoped<ISendMailService, SendMailService>();
        builder.Services.AddScoped<IMailResponseHandlerService, MailResponseHandlerService>();
        builder.Services.AddScoped<IEvenementService, EvenementService>();
        builder.Services.AddScoped<IDonatieService, DonatieService>();
        builder.Services.AddScoped<IBihzUserService, BihzUserService>();
        builder.Services.AddScoped<IBihzActieService, BihzActieService>();
        builder.Services.AddScoped<IBihzProjectService, BihzProjectService>();
        builder.Services.AddScoped<IBihzDonatieService, BihzDonatieService>();
        builder.Services.AddScoped<IBihzDonatieService, BihzDonatieService>();

        string syncFusionLicenseKey = builder.Configuration.GetValue<string>("SyncfusionConfiguration:LicenseKey");
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncFusionLicenseKey);

        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddSignalR(e =>
        {
            e.MaximumReceiveMessageSize = 10240000;
        });
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(GetDatabaseConnectionString(builder), po => po.EnableRetryOnFailure()));

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // Configure Mailjet client.
        builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
        {
            //set BaseAddress, MediaType, UserAgent
            client.SetDefaultSettings();

            string apiKey = builder.Configuration.GetValue<string>("MailJetConfiguration:ApiKey");
            string apiSecret = builder.Configuration.GetValue<string>("MailJetConfiguration:ApiSecret");
            client.UseBasicAuthentication(apiKey, apiSecret);
        });
    }

    static void UseServices(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // recommended to deactivate HTTPS redirection middleware in development
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        // make redirect URIs and other security policies work in production, see:
        // see https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-6.0
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
    }

    static IResult HandleNewAction(BihzActie action, IBihzActieService service, HttpRequest req)
    {
        return Results.Ok("Ik heb n Action toegevoegd");
    }

    static IResult HandleNewDonatie(BihzDonatie donation, IBihzDonatieService service)
    {
        service.Add(donation);
        return Results.Ok("Ik heb n Donation toegevoegd");
    }

    static IResult HandleNewProject(BihzProject project, IBihzProjectService service)
    {
        service.Add(project);
        return Results.Ok("Ik heb n Project toegevoegd");
    }

    static IResult HandleNewUser(BihzUser user, IBihzUserService service)
    {
        service.Add(user);
        return Results.Ok("Ik heb n User toegevoegd");
    }
}
