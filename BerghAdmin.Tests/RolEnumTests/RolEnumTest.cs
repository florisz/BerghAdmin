using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BerghAdmin.Data;
using System;

namespace BerghAdmin.Tests.EnumTests
{
    [TestFixture]
    public class RolEnumTests
    {
        [Test]
        public void TestTextIsEmpty()
        {
            var rolData = GetAlleRollen();
            Assert.That(9 == rolData.Length);
        }

        private static RolData[] GetAlleRollen()
            =>
            Enum.GetNames<RolTypeEnum>()
                .Select(t => new RolData { Text = t })
                .ToArray();
    }

    public class RolData
    {
        public string Text { get; set; } = "admin";
        public int Id { get; set; } = 1;
    }
}
