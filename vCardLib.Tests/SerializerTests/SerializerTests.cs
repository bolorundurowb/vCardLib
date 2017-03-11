using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Helpers;
using vCardLib.Serializers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Tests.SerializerTests
{
    [TestFixture]
    public class SerializerTests
    {
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void SerializeVcardTest()
        {
            string filePath = Path.Combine(assemblyFolder, "v3.vcf");
            vCard vcard = null;
            Assert.Throws<InvalidOperationException>(delegate
            {
                Serializer.Serialize(vcard, filePath, Version.V3);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                Serializer.Serialize(vcard, filePath, Version.V3, WriteOptions.Overwrite);
            });

            vcard = new vCard();
            filePath = Path.Combine(assemblyFolder, "testV2.vcf");
            Assert.DoesNotThrow(delegate
            {
                Serializer.Serialize(vcard, filePath, Version.V2);
            });
            FileAssert.Exists(filePath);

            filePath = Path.Combine(assemblyFolder, "testV3.vcf");
            Assert.DoesNotThrow(delegate
            {
                Serializer.Serialize(vcard, filePath, Version.V3);
            });
            FileAssert.Exists(filePath);

            filePath = Path.Combine(assemblyFolder, "testV4.vcf");
            Assert.Throws<NotImplementedException>(delegate
            {
                Serializer.Serialize(vcard, filePath, Version.V4);
            });
        }
    }
}