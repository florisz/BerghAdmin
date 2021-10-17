using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using BerghAdmin.Areas.Identity;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;

using Syncfusion.Blazor;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Context;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using BerghAdmin.Services.Configuration;

namespace BerghAdmin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<IPersoonService, PersoonService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ISeedDataService, SeedDataService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IMergeService, MergeService>();
            services.AddScoped<IDataImporterService, DataImporterService>();
            services.AddScoped<IContextService, ContextService>();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddSyncfusionBlazor();
            services.AddSignalR(e => 
            { 
                e.MaximumReceiveMessageSize = 10240000; 
            });
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(GetDatabaseConnectionString(), po => po.EnableRetryOnFailure()));

            /* enable next lines for authenticated access only
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            */
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

        private string GetDatabaseConnectionString()
        {
            var databaseConfiguration = Configuration.GetSection("DatabaseConfiguration").Get<DatabaseConfiguration>();
            if (databaseConfiguration == null)
            {
                throw new ApplicationException("Secrets for Database access (connection string & password) can not be found in configuration");
            }
            return databaseConfiguration.ConnectionString;
        }
    }
}
