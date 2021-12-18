using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.Context;
using BerghAdmin.Services.Import;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

RegisterAuthorization();
RegisterServices();

var app = builder.Build();
UseServices();

app.Run();


string GetDatabaseConnectionString()
{
    var databaseConfiguration = builder.Configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
    if (databaseConfiguration == null)
    {
        throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
    }
    return databaseConfiguration.ConnectionString ?? throw new ArgumentNullException("ConnectionString");
}

void RegisterAuthorization()
{
    //builder.Services.AddScoped<UserManager>();
    //builder.Services.AddScoped<IUserValidator<User>, UserValidator<User>>();
    //builder.Services.AddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
    //builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    //builder.Services.AddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
    ////builder.Services.AddScoped<IRoleValidator<TRole>, RoleValidator<TRole>>();
    //builder.Services.AddScoped<IdentityErrorDescriber>();
    //builder.Services.AddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
    //builder.Services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<User>>();
    ////builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, TRole>>();
    //builder.Services.AddScoped<UserManager<User>>();
    //builder.Services.AddScoped<SignInManager<User>>();
    //builder.Services.AddScoped<RoleManager<TRole>>();


    builder.Services
        .AddDefaultIdentity<User>(options => {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        })
        //.AddUserManager<UserManager>()
        //.AddSignInManager<UserManager>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.AddSingleton<IAuthorizationHandler, AdministratorPolicyHandler>();
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("IsAdministrator", policy => policy.Requirements.Add(new IsAdministratorRequirement()));
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
    //builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
    builder.Services.AddScoped<IPersoonService, PersoonService>();
    builder.Services.AddScoped<IRolService, RolService>();
    builder.Services.AddScoped<ISeedDataService, SeedDataService>();
    builder.Services.AddScoped<IDocumentService, DocumentService>();
    builder.Services.AddScoped<IDocumentMergeService, DocumentMergeService>();
    builder.Services.AddScoped<IDataImporterService, DataImporterService>();
    builder.Services.AddScoped<IContextService, ContextService>();
    builder.Services.AddScoped<ISendMailService, SendMailService>();
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