﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Deserializers;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Utils;

namespace vCardLib.Tests.DeserializerTests
{
    [TestFixture]
    public class DeserializerTests
    {
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void FromFileTest()
        {
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            List<vCard> collection = null;
            Assert.DoesNotThrow(delegate { collection = Deserializer.FromFile(filePath); });
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void FromStreamReaderTest()
        {
            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
            StreamReader streamReader = null;
            Assert.DoesNotThrow(delegate { streamReader = Helper.GetStreamReaderFromFile(filePath); });
            Assert.IsNotNull(streamReader);
            List<vCard> collection = null;
            Assert.DoesNotThrow(delegate { collection = Deserializer.FromStreamReader(streamReader); });
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void GetVcardFromDetailsNullTest()
        {
            string[] details = null;
            Assert.Throws<InvalidDataException>(delegate { Deserializer.GetVcardFromDetails(details); });
        }

        [Test]
        public void GetVcardFromDetailsValidDataTest()
        {
            var details = new[]
            {
                "VERSION:2.1"
            };
            vCard vcard = null;
            Assert.DoesNotThrow(delegate { vcard = Deserializer.GetVcardFromDetails(details); });
            Assert.AreEqual(vCardVersion.V2, vcard.Version);

            details = new[]
            {
                "VERSION:3.0"
            };
            vcard = null;
            Assert.DoesNotThrow(delegate { vcard = Deserializer.GetVcardFromDetails(details); });
            Assert.AreEqual(vCardVersion.V3, vcard.Version);

            details = new[]
            {
                "VERSION:4.0"
            };
            Assert.Throws<NotImplementedException>(delegate { vcard = Deserializer.GetVcardFromDetails(details); });
        }

        [Test]
        public void DeserializeCardWithCustomFields()
        {
            var filePath = Path.Combine(assemblyFolder, "custom-fields.vcf");
            List<vCard> collection = null;
            Assert.DoesNotThrow(delegate { collection = Deserializer.FromFile(filePath); });
            Assert.AreEqual(1, collection.Count);
            var vcard = collection[0];
            Assert.AreEqual(vcard.CustomFields.Count, 5);
        }
    }
}
