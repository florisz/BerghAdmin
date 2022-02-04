using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BerghAdmin.Data;
using System;
using BerghAdmin.Services.Betalingen;

namespace BerghAdmin.Tests.BetalingenTests
{
    [TestFixture]
    public class BetalingenTests
    {
        [Test]
        public void TestImportRaboBetalingenBestand()
        {
            var betalingenImporterService = new BetalingenImporterService();

            using (FileStream fs = File.OpenRead("BetalingenTests/TestRaboBetalingenBestand.csv"))
            {
                betalingenImporterService.ImportData(fs);
            }
            
            // TO BE DONE: continue when betalingen service is done
            Assert.IsTrue(true);
        }
    }
}
