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

        [TestCase(1, RolTypeEnum.Contactpersoon)]
        [TestCase(2, RolTypeEnum.Begeleider)]
        [TestCase(3, RolTypeEnum.CommissieLid)]
        [TestCase(4, RolTypeEnum.Fietser)]
        [TestCase(5, RolTypeEnum.Golfer)]
        [TestCase(6, RolTypeEnum.MailingAbonnee)]
        [TestCase(7, RolTypeEnum.VriendVan)]
        [TestCase(8, RolTypeEnum.Vrijwilliger)]
        public void TestRolTypeEnum(int val, RolTypeEnum enm)
            => Assert.AreEqual(val, (int)enm);

    }
}

