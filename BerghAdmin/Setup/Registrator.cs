using Azure.Identity;

using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Betalingen;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Seeding;
using BerghAdmin.Services.Sponsoren;
using BerghAdmin.Services.UserManagement;
using Mailjet.Client;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using Serilog;

using Syncfusion.Blazor;

using System.IO.Abstractions;

namespace BerghAdmin;

public class Registrator
{
    private WebApplicationBuilder _builder;
    private WebApplication? _app = null;

    public Registrator(WebApplicationBuilder builder)
    {
        this._builder = builder;
    }

    public void RegisterAuthorization()
    {
        var services = this._builder.Services;
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
        _builder.Configuration.AddUserSecrets<Program>();
        if (env != "Development")
        {
            var vaultName = Environment.GetEnvironmentVariable("VaultName");
            _builder.Configuration.AddAzureKeyVault(
              new Uri($"https://{vaultName}.vault.azure.net"),
              new DefaultAzureCredential()
              );
        }

        // Add services to the container.
        _builder.Services.AddRazorPages();
        _builder.Services.AddServerSideBlazor();
        _builder.Services.AddOptions();
        _builder.Services.AddHttpClient();
        _builder.Services.Configure<SeedSettings>(_builder.Configuration.GetSection("Seeding"));
        _builder.Services.Configure<ApiConfiguration>(_builder.Configuration.GetSection("ApiConfiguration"));
        _builder.Services.AddSingleton<EndpointHandler>();
        _builder.Services.AddScoped<IBetalingenRepository, EFBetalingenRepository>();
        _builder.Services.AddScoped<IBetalingenService, BetalingenService>();
        _builder.Services.AddScoped<IBetalingenImporterService, BetalingenImporterService>();
        _builder.Services.AddScoped<IPersoonService, PersoonService>();
        _builder.Services.AddScoped<IRolService, RolService>();
#if (DEBUG)
        _builder.Services.AddTransient<ISeedDataService, DebugSeedDataService>();
#elif (RELEASE)
        _builder.Services.AddTransient<ISeedDataService, ReleaseSeedDataService>();
#endif
        _builder.Services.AddTransient<ISeedUsersService, ReleaseSeedUsersService>();
        _builder.Services.AddScoped<IDocumentService, DocumentService>();
        _builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
        _builder.Services.AddScoped<IDataImporterService, DataImporterService>();
        _builder.Services.AddScoped<IFileSystem, FileSystem>();
        _builder.Services.AddScoped<IMailAttachmentsService>((provider) =>
        {
            var rootPath = Path.Combine(_builder.Environment.ContentRootPath, "wwwroot");
            return new MailAttachmentsService(rootPath,
                provider.GetRequiredService<IFileSystem>(),
                provider.GetRequiredService<ILogger<MailAttachmentsService>>());
        });
        _builder.Services.AddScoped<ISendMailService, SendMailService>();
        _builder.Services.AddScoped<IFietstochtenService, FietstochtenService>();
        _builder.Services.AddScoped<IGolfdagenService, GolfdagenService>();
        _builder.Services.AddScoped<ISponsorService, SponsorService>();
        _builder.Services.AddScoped<IDonatieService, DonatieService>();
        _builder.Services.AddScoped<IUserService, UserService>();
        _builder.Services.AddScoped<IBihzUserService, BihzUserService>();
        _builder.Services.AddScoped<IBihzActieService, BihzActieService>();
        _builder.Services.AddScoped<IBihzProjectService, BihzProjectService>();
        _builder.Services.AddScoped<IBihzDonatieService, BihzDonatieService>();
        _builder.Services.AddScoped<IBihzDonatieService, BihzDonatieService>();

        var syncFusionLicenseKey = _builder.Configuration.GetValue<string>("SyncfusionConfiguration:LicenseKey");
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncFusionLicenseKey);

        _builder.Services.AddSyncfusionBlazor();
        _builder.Services.AddSignalR(e =>
        {
            e.MaximumReceiveMessageSize = 10240000;
        });

        var serverVersion = ServerVersion.Parse("5.7");
        Action<MySqlDbContextOptionsBuilder> option = o => o.EnableRetryOnFailure();

#if DEBUG
        _builder.Services.AddDbContextFactory<ApplicationDbContext>(
                    options => options
                            .UseMySql(GetDatabaseConnectionString(_builder), serverVersion, option)
                            .EnableSensitiveDataLogging(true)
                            .EnableDetailedErrors(true));
#else
        _builder.Services.AddDbContextFactory<ApplicationDbContext>(
                    options => options
                            .UseMySql(GetDatabaseConnectionString(_builder), serverVersion, option));
#endif
        _builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseMySql(GetDatabaseConnectionString(_builder), serverVersion, option), 
                    optionsLifetime : ServiceLifetime.Singleton);

        _builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        // Configure Mailjet client.
        _builder.Services.AddHttpClient<IMailjetClient, MailjetClient>(client =>
        {
            //set BaseAddress, MediaType, UserAgent
            client.SetDefaultSettings();

            string? apiKey = _builder.Configuration["MailJetConfiguration:ApiKey"];
            string? apiSecret = _builder.Configuration["MailJetConfiguration:ApiSecret"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new ApplicationException("Secrets for MailJet access (ApiKey & ApiSecret) cannot be found in configuration");
            }
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

#pragma warning disable ASP0014
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });
#pragma warning restore ASP0014

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
        => _app ??= this._builder.Build();
}
