using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserializers;
using vCardLib.Enums;

namespace vCardLib.Tests
{
    [TestFixture]
    public class DeserializerTests
    {
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void ShouldSucceedWithV2File()
        {
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            var cards = Deserializer.FromFile(filePath);
            cards.Count.ShouldBe(1);

            var card = cards.FirstOrDefault();

            card.ShouldNotBeNull();
            card.Version.ShouldBe(vCardVersion.V2);
            card.EmailAddresses.Count.ShouldBe(8);
            card.PhoneNumbers.Count.ShouldBe(15);
        }

        [Test]
        public void ShouldSucceedWithV3File()
        {
            var filePath = Path.Combine(assemblyFolder, "v3.vcf");
            var cards = Deserializer.FromFile(filePath);
            cards.Count.ShouldBe(1);

            var card = cards.FirstOrDefault();

            card.ShouldNotBeNull();
            card.Version.ShouldBe(vCardVersion.V3);
            card.EmailAddresses.Count.ShouldBe(9);
            card.PhoneNumbers.Count.ShouldBe(17);
        }

        [Test]
        public void ShouldThrowWithV4File()
        {
            var filePath = Path.Combine(assemblyFolder, "v4.vcf");
            Assert.Throws<NotImplementedException>(delegate { Deserializer.FromFile(filePath); });
        }

        [Test]
        public void ShouldThrowWithInvalidFile()
        {
            var filePath = Path.Combine(assemblyFolder, "invalid.vcf");
            Assert.Throws<InvalidOperationException>(delegate { Deserializer.FromFile(filePath); });
        }

        [Test]
        public void ShouldSucceedWithStream()
        {
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var cards = Deserializer.FromStream(stream);
            Assert.AreEqual(1, cards.Count);
        }

        [Test]
        public void ShouldSucceedWithStringContent()
        {
            var details = @"
BEGIN:VCARD
VERSION:2.1
FN:James Doe
END:VCARD
";
            var cards = Deserializer.FromString(details);
            Assert.AreEqual(1, cards.Count);

            var card = cards.First();
            Assert.AreEqual("James Doe", card.FormattedName);
        }

        [Test]
        public void DeserializeCardWithCustomFields()
        {
            var filePath = Path.Combine(assemblyFolder, "custom-fields.vcf");
            var cards = Deserializer.FromFile(filePath);
            Assert.AreEqual(1, cards.Count);

            var card = cards.First();
            Assert.AreEqual(5, card.CustomFields.Count);
        }
    }
}