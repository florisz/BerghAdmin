using BerghAdmin.Services;
using BerghAdmin.Services.DateTimeProvider;
using BerghAdmin.Services.Export;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace BerghAdmin.Tests.ExportTests
{
    [TestFixture]
    public class ExcelTests : DatabaseTestSetup
    {
        protected override void RegisterServices(ServiceCollection services)
        {
            services
                .AddScoped<IExcelService, ExcelService>()
                .AddScoped<IPersoonService, PersoonService>()
                .AddScoped<IRolService, RolService>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }

        [Test]
        public async Task ExportPersonenTest()
        {
            var excelService = this.GetRequiredService<IExcelService>();
            var rv = await excelService.ExportPersonenAsync("not/a/valid/filename");

            Assert.IsFalse(rv);
        }

    }
}
