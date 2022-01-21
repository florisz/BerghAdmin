using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Syncfusion.Blazor;

using System.Text;

namespace BerghAdmin;

public class Program
{
    public class ConsoleLogger : TextWriter
    {
        private readonly ILogger logger;

        public ConsoleLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public override Encoding Encoding => Encoding.UTF8;
        public override void WriteLine(string? value)
        {
            this.logger.LogInformation(value);
        }
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //builder.Logging.ClearProviders();
        //builder.Logging.Add

        // Redirect all Console output to the logger
        Console.SetOut(new ConsoleLogger(new LoggerFactory().CreateLogger("aap")));

        RegisterAuthorization(builder.Services);
        RegisterServices(builder);

        var app = builder.Build();
        UseServices(app);

        app.MapPost("/donaties",
            (Donatie donatie, IKentaaService service) => HandleNewDonatie(donatie, service))
            .AllowAnonymous();


        var seedService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedDataService>();
        seedService.SeedInitialData();

        app.Run();
    }

    static string GetDatabaseConnectionString(WebApplicationBuilder builder)
    {
        Console.WriteLine("Getting the ConnectionString from the configuration");

        var databaseConfiguration = builder.Configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
        if (databaseConfiguration == null)
        {
            throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
        }
        return databaseConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString not specified");
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

        services.AddSingleton<IAuthorizationHandler, AdministratorPolicyHandler>();
        services.AddSingleton<IAuthorizationHandler, BeheerFietsersPolicyHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("IsAdministrator", policy => policy.Requirements.Add(new IsAdministratorRequirement()));
            options.AddPolicy("BeheerFietsers", policy => policy.Requirements.Add(new IsFietsersBeheerderRequirement()));
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
        builder.Services.AddScoped<IPersoonService, PersoonService>();
        builder.Services.AddScoped<IRolService, RolService>();
        builder.Services.AddTransient<ISeedDataService, SeedDataService>();
        builder.Services.AddScoped<IDocumentService, DocumentService>();
        builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
        builder.Services.AddScoped<IDataImporterService, DataImporterService>();
        builder.Services.AddScoped<ISendMailService, SendMailService>();
        builder.Services.AddScoped<IEvenementService, EvenementService>();
        builder.Services.AddScoped<IDonatieService, DonatieService>();
        builder.Services.Configure<MailJetConfiguration>(builder.Configuration.GetSection("MailJetConfiguration"));
        builder.Services.AddScoped<IKentaaService, KentaaService>();
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddSignalR(e =>
        {
            e.MaximumReceiveMessageSize = 10240000;
        });
        builder.Services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlServer(GetDatabaseConnectionString(builder), po => po.EnableRetryOnFailure()));

    }

    static void UseServices(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

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

    static IResult HandleNewDonatie(Donatie donatie, IKentaaService service)
    {
        service.AddDonation(new KentaaDonatie());
        return Results.Ok("Ik heb m toegevoegd");
    }
}

