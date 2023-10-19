using Azure.Identity;

using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Betalingen;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Seeding;
using BerghAdmin.Services.UserManagement;
using Mailjet.Client;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Serilog;

using Syncfusion.Blazor;

using System.IO.Abstractions;

namespace BerghAdmin;

public class Registrator
{
    private WebApplicationBuilder builder;
    private WebApplication? app = null;

    public Registrator(WebApplicationBuilder builder)
    {
        this.builder = builder;
    }

    public void RegisterAuthorization()
    {
        var services = this.builder.Services;
        services
            .AddDefaultIdentity<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedPhoneNumber = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddUserManager<UserManager<User>>()
            .AddSignInManager<SignInManager<User>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddSingleton<IAuthorizationHandler, AdministratorPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerFietsersPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerAmbassadeursPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerFinancienPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerGolfersPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerSecretariaatPolicyHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdministrator", policy => policy.Requirements.Add(new IsAdministratorRequirement()));
            options.AddPolicy("IsSecretariaat", policy => policy.Requirements.Add(new IsSecretariaatBeheerderRequirement()));
            options.AddPolicy("IsBeheerAmbassadeurs", policy => policy.Requirements.Add(new IsAmbassadeursBeheerderRequirement()));
            options.AddPolicy("IsBeheerFietsers", policy => policy.Requirements.Add(new IsFietsersBeheerderRequirement()));
            options.AddPolicy("IsBeheerFinancien", policy => policy.Requirements.Add(new IsFinancienBeheerderRequirement()));
            options.AddPolicy("IsBeheerGolfers", policy => policy.Requirements.Add(new IsGolfersBeheerderRequirement()));
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

    }

    public WebApplication BuildApp()
        => GetApp();

    public void SetupDatabase()
    {
        var app = GetApp();

        var seedDataService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedDataService>();
        seedDataService.SeedInitialData();
        var seedUsersService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedUsersService>();
        seedUsersService.SeedUsersData();
    }

    public void RegisterServices()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (string.IsNullOrEmpty(env))
            throw new ArgumentNullException("ASPNETCORE_ENVIRONMENT");

        //builder.Configuration.AddUserSecrets<SendMailService>();
        builder.Configuration.AddUserSecrets<Program>();
        if (env != "Development")
        {
            var vaultName = Environment.GetEnvironmentVariable("VaultName");
            builder.Configuration.AddAzureKeyVault(
              new Uri($"https://{vaultName}.vault.azure.net"),
              new DefaultAzureCredential()
              );
        }

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddOptions();
        builder.Services.AddHttpClient();
        builder.Services.Configure<SeedSettings>(builder.Configuration.GetSection("Seeding"));
        builder.Services.Configure<ApiConfiguration>(builder.Configuration.GetSection("ApiConfiguration"));
        builder.Services.AddSingleton<EndpointHandler>();
        builder.Services.AddScoped<IBetalingenRepository, EFBetalingenRepository>();
        builder.Services.AddScoped<IBetalingenService, BetalingenService>();
        builder.Services.AddScoped<IBetalingenImporterService, BetalingenImporterService>();
        builder.Services.AddScoped<IPersoonService, PersoonService>();
        builder.Services.AddScoped<IRolService, RolService>();
#if (DEBUG)
        builder.Services.AddTransient<ISeedDataService, DebugSeedDataService>();
#elif (RELEASE)
        builder.Services.AddTransient<ISeedDataService, ReleaseSeedDataService>();
#endif
        builder.Services.AddTransient<ISeedUsersService, ReleaseSeedUsersService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
        builder.Services.AddScoped<IDataImporterService, DataImporterService>();
        builder.Services.AddScoped<IFileSystem, FileSystem>();
        builder.Services.AddScoped<IMailAttachmentsService>((provider) =>
        {
            var rootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
            return new MailAttachmentsService(rootPath,
                provider.GetRequiredService<IFileSystem>(),
                provider.GetRequiredService<ILogger<MailAttachmentsService>>());
        });
        builder.Services.AddScoped<ISendMailService, SendMailService>();
        builder.Services.AddScoped<IEvenementService, EvenementService>();
        builder.Services.AddScoped<IDonatieService, DonatieService>();
        builder.Services.AddScoped<IUserService, UserService>();
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
            options => options.UseMySql(GetDatabaseConnectionString(builder), ServerVersion.Parse("5.7"), po => po.EnableRetryOnFailure()));
        // TOBEDONE
        //        builder.Services.AddDbContextFactory<ContactContext>(
        //            options => options.UseMySql(GetDatabaseConnectionString(builder), ServerVersion.Parse("5.7"), po => po.EnableRetryOnFailure()));
        
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // Configure Mailjet client.
        builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
        {
            //set BaseAddress, MediaType, UserAgent
            client.SetDefaultSettings();

            string apiKey = builder.Configuration["MailJetConfiguration:ApiKey"];
            string apiSecret = builder.Configuration["MailJetConfiguration:ApiSecret"];
            client.UseBasicAuthentication(apiKey, apiSecret);
        });
    }

    public void UseServices()
    {
        var app = GetApp();
        if (app.Environment.IsDevelopment())
        {
            // recommended to deactivate HTTPS redirection middleware in development
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler(MyExceptionHandler);
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

        var handler = app.Services.GetRequiredService<EndpointHandler>();
        handler.CreateEndpoints(app);
    }

    private void MyExceptionHandler(IApplicationBuilder handlerApp)
    {
        handlerApp.Run(async context =>
        {
            var msg = new MailMessage();
            msg.To.Add(new MailAddress("fzwarteveen@gmail.com", "Floris"));
            msg.Subject = "BerghAdmin Exception";
            msg.TextBody = $@"Dit is waar het verkeerd gegaan is:
                                        User={context.User.Identity?.Name}
                                        Url={context.Request.Path}
                                        Exception=";

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                msg.TextBody += exceptionHandlerPathFeature.Error.ToString();
            }

            Log.Logger.Error(msg.TextBody);

            var mailService = handlerApp.ApplicationServices.GetRequiredService<ISendMailService>();
            await mailService.SendMail(msg);

            context.Response.Redirect("/Error");
        });
    }

    private string GetDatabaseConnectionString(WebApplicationBuilder builder)
    {
        var cs = builder.Configuration.GetConnectionString("DefaultConnection");
        if (cs == null)
        {
            throw new ApplicationException("Secrets for Database access (connection string & password) cannot be found in configuration");
        }

        //Log.Logger.Information($"DatabaseConnectionString={cs}");

        return cs;
    }

    private WebApplication GetApp()
        => app ??= this.builder.Build();
}
