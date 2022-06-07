using Serilog;

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

        registrator.UseServices();
        registrator.SetupDatabase();

        var app = registrator.BuildApp();
        app.Run();
    }
}
