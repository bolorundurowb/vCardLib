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
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void FromFileTest()
        {
            string filePath = Path.Combine(assemblyFolder, "v2.vcf");
            vCardCollection collection = null;
            Assert.DoesNotThrow(delegate
            {
                collection = Deserializer.FromFile(filePath);
            });
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void FromStreamReaderTest()
        {
            string filePath = Path.Combine(assemblyFolder, "v2.vcf");
            StreamReader streamReader = null;
            Assert.DoesNotThrow(delegate
            {
                streamReader = Helper.GetStreamReaderFromFile(filePath);
            });
            Assert.IsNotNull(streamReader);
            vCardCollection collection = null;
            Assert.DoesNotThrow(delegate
            {
                collection = Deserializer.FromStreamReader(streamReader);
            });
            Assert.AreEqual(1, collection.Count);
        }
    }
}