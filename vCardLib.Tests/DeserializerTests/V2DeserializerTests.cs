﻿using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Deserializers;
using vCardLib.Models;
using vCardLib.Utils;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class V2DeserializerTests
    {
        [Test]
        public void ParseTest()
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            var streamReader = Helper.GetStreamReaderFromFile(filePath);
            var contactsString = Helper.GetStringFromStreamReader(streamReader);
            var contacts = Helper.GetContactsArrayFromString(contactsString);
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