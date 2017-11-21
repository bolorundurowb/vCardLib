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
				foreach ( var contact in contacts )
				{
					var details = Helper.GetContactDetailsArrayFromString( contact );
					vcard = V2Deserializer.Parse( details, vcard );
					Assert.IsNotNull( vcard );
				}
            });
        }
    }
}