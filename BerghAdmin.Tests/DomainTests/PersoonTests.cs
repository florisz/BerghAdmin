using NUnit.Framework;
using System.Collections.Generic;
using BerghAdmin.Data;
using System.Linq;

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
            Assert.AreEqual(null, persoon.GetRollenAsString);
        }
        
        [Test]
        public void TestGetRollenOneRol()
        {
            var persoon = new Persoon { Id = 1, 
                                        Rollen = new HashSet<Rol> 
                                            { 
                                                new Rol { Id = RolTypeEnum.Ambassadeur,Beschrijving = "Aap" } 
                                            } 
                                      };
            Assert.AreEqual("Aap", persoon.GetRollenAsString);
        }
        
        [Test]
        public void TestGetRollenMoreRollen()
        {
            var persoon = new Persoon { Id = 1, 
                                        Rollen = new HashSet<Rol> 
                                            { 
                                                new Rol { Id = RolTypeEnum.Ambassadeur, Beschrijving = "Aap" }, 
                                                new Rol { Id = RolTypeEnum.Fietser, Beschrijving = "Noot" }, 
                                                new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Mies" }, 
                                            } 
                                      };
            Assert.AreEqual("Aap, Noot, Mies", persoon.GetRollenAsString);
        }

        // Actually not a real test but a search for the correct lambda
        // Test can be deleted somewhere in the future
        [Test]
        public void TestNotARealTestButALambdaTest()
        {
            var rol1 = new Rol { Id = RolTypeEnum.Ambassadeur, Beschrijving = "Ambassadeur" };
            var rol2 = new Rol { Id = RolTypeEnum.Fietser, Beschrijving = "Fietser" };
            var rol3 = new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Golfer" };
            var rol4 = new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Vrijwilliger" };
            var persoon1 = new Persoon { Id = 1, Rollen = new HashSet<Rol> { rol1, rol2, rol3, rol4 } };
            var persoon2 = new Persoon { Id = 2, Rollen = new HashSet<Rol> { rol2, rol4 } };
            var persoon3 = new Persoon { Id = 3, Rollen = new HashSet<Rol> { rol3, rol4 } };
            var persoon4 = new Persoon { Id = 4, Rollen = new HashSet<Rol> { rol4 } };
            var personen = new List<Persoon> { persoon1, persoon2, persoon3, persoon4 };
            
            var personenList = PersonenWithRollen(personen, new List<Rol> { rol1 });
            Assert.AreEqual(1, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol2 });
            Assert.AreEqual(2, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol3 });
            Assert.AreEqual(2, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol4 });
            Assert.AreEqual(4, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol1, rol2 });
            Assert.AreEqual(2, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol2, rol3 });
            Assert.AreEqual(3, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol1, rol3 });
            Assert.AreEqual(2, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol1, rol2, rol3 });
            Assert.AreEqual(3, personenList.Count);
            personenList = PersonenWithRollen(personen, new List<Rol> { rol1, rol2, rol3, rol4 });
            Assert.AreEqual(4, personenList.Count);
        }

        private List<Persoon> PersonenWithRollen(List<Persoon> personen, List<Rol> rollen)
        {
            return personen
                    .Where(p => p.Rollen.FirstOrDefault(r => rollen.Contains(r) ) != null) 
                    .ToList<Persoon>();
        }

    }
}

