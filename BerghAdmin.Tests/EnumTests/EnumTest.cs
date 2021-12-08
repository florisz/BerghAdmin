using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using BerghAdmin.Data;
using System;

namespace BerghAdmin.Tests.EnumTests
{
    [TestFixture]
    public class MailMergeTests
    {
        [Test]
        public void TestTextIsEmpty()
        {
            var rolData = GetAlleRollen();
            Assert.AreEqual(9, rolData.Length);
        }

        private static RolData[] GetAlleRollen()
        {
            var rolData = new List<RolData>();

            foreach (int rolType in Enum.GetValues(typeof(RolTypeEnum)))
            {
                var isEnumParsed = Enum.TryParse(rolType.ToString(), true, out RolTypeEnum parsedEnumValue);

                if (isEnumParsed)
                {
                    rolData.Add(new RolData() { Id = rolType, Text = parsedEnumValue.ToString() });
                }
            }

            return rolData.ToArray<RolData>();
        }


    }

    public class RolData
    {
        public string Text { get; set; } = "admin";
        public int Id { get; set; } = 1;
    }
}
