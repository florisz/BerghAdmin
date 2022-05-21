using Azure.Identity;

using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Betalingen;
using BerghAdmin.Services.Bihz;
using BerghAdmin.Services.Donaties;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Import;
using BerghAdmin.Services.Seeding;

using Mailjet.Client;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Serilog;

using Syncfusion.Blazor;

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

        var registrator = new Registrator(builder);

        registrator.RegisterAuthorization();
        registrator.RegisterServices();
        
        var app = registrator.BuildApp();
        registrator.UseServices();
        registrator.SetupDatabase();

        app.Run();
    }
}
