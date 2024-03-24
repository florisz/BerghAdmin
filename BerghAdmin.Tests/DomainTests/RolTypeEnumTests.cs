using NUnit.Framework;
using BerghAdmin.Data;

//
// just to be absolutely sure the enum values are never changed
//
namespace BerghAdmin.Tests.DomainTests;

[TestFixture]
public class RolTypeEnumTests
{
    [SetUp]
    public void SetupPersoonTests()
    {
    }        

    [TestCase(1, RolTypeEnum.Compagnon)]
    [TestCase(2, RolTypeEnum.Contactpersoon)]
    [TestCase(3, RolTypeEnum.Begeleider)]
    [TestCase(4, RolTypeEnum.CommissieLid)]
    [TestCase(5, RolTypeEnum.Fietser)]
    [TestCase(6, RolTypeEnum.Golfer)]
    [TestCase(7, RolTypeEnum.MailingAbonnee)]
    [TestCase(8, RolTypeEnum.VriendVan)]
    [TestCase(9, RolTypeEnum.Vrijwilliger)]
    public void TestRolTypeEnum(int val, RolTypeEnum enm)
        => Assert.AreEqual(val, (int)enm);

}

