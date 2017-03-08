using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Deserializers;
using vCardLib.Helpers;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class DeserializerTests
    {
        [Test]
        public void FromFileTest()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "v2.vcf");
            vCardCollection collection = null;
            Assert.DoesNotThrow(delegate
            {
                collection = Deserializer.FromFile(filePath);
            });
            Assert.AreEqual(1, collection.Count);
        }
    }
}