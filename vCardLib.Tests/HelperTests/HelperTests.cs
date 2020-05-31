// Created by Bolorunduro Winner-Timothy on  11/25/2016 at 7:43 AM

using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Utils;

namespace vCardLib.Tests.HelperTests
{
    [TestFixture]
    public class HelperTests
    {
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void GetStreamReaderTest()
        {
            string filePath = null;
            Assert.Throws<ArgumentNullException>(delegate { Helpers.GetStreamReaderFromFile(filePath); });
            filePath = string.Empty;
            Assert.Throws<ArgumentNullException>(delegate { Helpers.GetStreamReaderFromFile(filePath); });
            filePath = @"C:\Test.vcf";
            Assert.Throws<FileNotFoundException>(delegate { Helpers.GetStreamReaderFromFile(filePath); });

            filePath = Path.Combine(assemblyFolder, "invalid.vcf");
            var streamReader = Helpers.GetStreamReaderFromFile(filePath);
            Assert.IsNotNull(streamReader);
        }

        [Test]
        public void GetContactsArrayFromStringTest()
        {
            var contactsString = "   ";
            Assert.Throws<ArgumentException>(delegate { Helpers.GetContactsArrayFromString(contactsString); });
            contactsString = "hello!";
            Assert.Throws<InvalidOperationException>(delegate { Helpers.GetContactsArrayFromString(contactsString); });
            contactsString = "BEGIN:VCARD\r\nEND:VCARD";
            string[] contacts = null;
            Assert.DoesNotThrow(delegate { contacts = Helpers.GetContactsArrayFromString(contactsString); });
            Assert.IsNotNull(contacts);
            Assert.IsTrue(contacts.Length > 0);
        }

        [Test]
        public void GetContactDetailsArrayFromStringTest()
        {
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            var streamReader = Helpers.GetStreamReaderFromFile(filePath);
            var vcardString = Helpers.GetStringFromStreamReader(streamReader);
            var contacts = Helpers.GetContactsArrayFromString(vcardString);
            string[] contactDetails = null;
            Assert.DoesNotThrow(delegate { contactDetails = Helpers.GetContactDetailsArrayFromString(contacts[0]); });
            Assert.IsNotNull(contactDetails);
            Assert.IsTrue(contactDetails.Length > 0);
        }
    }
}
