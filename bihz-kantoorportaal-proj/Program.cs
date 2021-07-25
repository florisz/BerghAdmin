using MailMerge;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bihz.kantoorportaal
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            // var mergeFields = new Dictionary<string, string> 
            // {
            //     { "Bedrijfsnaam", "Pieters en zn." },
            //     { "NaamAanhef", "de heer P. Pieters" },
            //     { "StraatEnNummer", "Pieterpad 12" },
            //     { "Postcode", "2961 XY" },
            //     { "Plaatsnaam", "Pieterbuuren" },
            //     { "HuidigeDatum", "24 juli 2021" },
            //     { "Sponsorbedrag", "€ 2.000,-" },
            // };
            // var (ok,errors) = new MailMerger().Merge(@"c:\temp\TemplateFactuurSponsor.docx", mergeFields, @"c:\temp\FactuurSponsor.docx");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
