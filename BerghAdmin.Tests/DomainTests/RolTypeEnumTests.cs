using NUnit.Framework;
using BerghAdmin.Data;

//
// just to be absolutely sure the enum values are never changed
//
namespace BerghAdmin.Tests.DomainTests
{
    [TestFixture]
    public class RolTypeEnumTests
    {
        [SetUp]
        public void SetupPersoonTests()
        {
        }        

        [Test]
        public void TestAmbassadeur() => Assert.AreEqual(1, RolTypeEnum.Ambassadeur);
        
        [Test]
        public void TestBegeleider() => Assert.AreEqual(2, RolTypeEnum.Begeleider);
        
        [Test]
        public void TestCommissieLid() => Assert.AreEqual(3, RolTypeEnum.CommissieLid);
        [Test]
        public void TestContactpersoon() => Assert.AreEqual(4, RolTypeEnum.Contactpersoon);
        [Test]
        public void TestFietser() => Assert.AreEqual(5, RolTypeEnum.Fietser);
        [Test]
        public void TestGolfer() => Assert.AreEqual(6, RolTypeEnum.Golfer);
        [Test]
        public void TestMailingAbonnee() => Assert.AreEqual(7, RolTypeEnum.MailingAbonnee);
        [Test]
        public void TestVriendVan() => Assert.AreEqual(8, RolTypeEnum.VriendVan);
        [Test]
        public void TestVrijwilliger() => Assert.AreEqual(9, RolTypeEnum.Vrijwilliger);
    }
}

