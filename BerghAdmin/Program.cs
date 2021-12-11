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

// Add services to the container.
builder.Services
    .AddDefaultIdentity<IdentityUser>(options => {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequiredLength = 12;
        options.Password.RequireDigit= false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>();
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

builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

var app = builder.Build();
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
