using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Deserializers;
using vCardLib.Helpers;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class V2DeserializerTests
    {
        [Test]
        public void ParseTest()
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "v2.vcf");
            StreamReader streamReader = Helper.GetStreamReaderFromFile(filePath);
            string contactsString = Helper.GetStringFromStreamReader(streamReader);
            string[] contacts = Helper.GetContactsArrayFromString(contactsString);
            vCard vcard = null;
            Assert.DoesNotThrow(delegate
            {
                vcard = V2Deserializer.Parse(contacts, vcard);
            });
            Assert.IsNotNull(vcard);
            //TODO: More assertions to check data integrity should be added
        }
    }
}