using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Context;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.KentaaInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

RegisterAuthorization();
RegisterServices();

var app = builder.Build();
UseServices();


var seedService = app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeedDataService>();
seedService.SeedInitialData();

app.Run();

string GetDatabaseConnectionString()
{
    var databaseConfiguration = builder.Configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
    if (databaseConfiguration == null)
    {
        throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
    }
    return databaseConfiguration.ConnectionString ?? throw new ArgumentException("ConnectionString not specified");
}

void RegisterAuthorization()
{
    builder.Services
        .AddDefaultIdentity<User>(options => {
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

    builder.Services.AddSingleton<IAuthorizationHandler, AdministratorPolicyHandler>();
    builder.Services.AddSingleton<IAuthorizationHandler, BeheerFietsersPolicyHandler>();
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("IsAdministrator", policy => policy.Requirements.Add(new IsAdministratorRequirement()));
        options.AddPolicy("BeheerFietsers", policy => policy.Requirements.Add(new IsFietsersBeheerderRequirement()));
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

}

void RegisterServices()
{
    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();

    builder.Services.AddOptions();
    builder.Services.Configure<SeedSettings>(builder.Configuration.GetSection("Seeding"));
    builder.Services.AddScoped<IPersoonService, PersoonService>();
    builder.Services.AddScoped<IRolService, RolService>();
    builder.Services.AddTransient<ISeedDataService, SeedDataService>();
    builder.Services.AddScoped<IDocumentService, DocumentService>();
    builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
    builder.Services.AddScoped<IDataImporterService, DataImporterService>();
    builder.Services.AddScoped<IContextService, ContextService>();
    builder.Services.AddScoped<ISendMailService, SendMailService>();
    builder.Services.AddScoped<IEvenementService, EvenementService>();
    builder.Services.AddScoped<IDonatieService, DonatieService>();
    builder.Services.Configure<KentaaConfiguration>(builder.Configuration.GetSection("KentaaConfiguration"));
    builder.Services.AddScoped<IKentaaService, KentaaService>();
    builder.Services.AddScoped<IKentaaInterfaceService, KentaaInterfaceService>();
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
    builder.Services.AddSyncfusionBlazor();
    builder.Services.AddSignalR(e =>
    {
        e.MaximumReceiveMessageSize = 10240000;
    });
    builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer(GetDatabaseConnectionString(), po => po.EnableRetryOnFailure()));

}

void UseServices()
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