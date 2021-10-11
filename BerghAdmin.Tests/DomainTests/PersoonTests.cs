using System;
using NUnit.Framework;
using System.Collections.Generic;
using BerghAdmin.Data;
using System.IO;

namespace BerghAdmin.Tests.DomainTests
{
    [TestFixture]
    public class PersoonTests
    {
        [SetUp]
        public void SetupPersoonTests()
        {
        }        

        [Test]
        public void TestGetRollenEmpty()
        {
            var persoon = new Persoon { Id = 1 };
            Assert.AreEqual(null, persoon.GetRollen);
        }
        
        [Test]
        public void TestGetRollenOneRol()
        {
            var persoon = new Persoon { Id = 1, 
                                        Rollen = new HashSet<Rol> 
                                            { 
                                                new Rol { Id = 1, Beschrijving = "Aap" } 
                                            } 
                                      };
            Assert.AreEqual("Aap", persoon.GetRollen);
        }
        
        [Test]
        public void TestGetRollenMoreRollen()
        {
            var persoon = new Persoon { Id = 1, 
                                        Rollen = new HashSet<Rol> 
                                            { 
                                                new Rol { Id = 1, Beschrijving = "Aap" }, 
                                                new Rol { Id = 2, Beschrijving = "Noot" }, 
                                                new Rol { Id = 3, Beschrijving = "Mies" }, 
                                            } 
                                      };
            Assert.AreEqual("Aap, Noot, Mies", persoon.GetRollen);
        }
    }
}

