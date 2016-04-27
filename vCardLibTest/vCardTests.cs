using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vCardLib;
using System.IO;

namespace vCardLibTest
{
    [TestClass]
    public class vCardTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void vCardWithNullFilePath()
        {
            string filePath = null;
            vCardCollection vcardCollection = vCard.FromFile(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void vCardWithEmptyFilePath()
        {
            string filePath = string.Empty;
            vCardCollection vcardCollection = vCard.FromFile(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void vCardWithNonExistentFilePath()
        {
            string filePath = @"C:\Test.vcf";
            vCardCollection vcardCollection = vCard.FromFile(filePath);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void vCardWithInvalidFileStructure()
        {
            string filePath = @"Test.txt";
            vCardCollection vcardCollection = vCard.FromFile(filePath);
        }

        [TestMethod]
        public void vCardWithValidFile()
        {
            string filePath = @"Test.vcf";
            vCardCollection vcardCollection = vCard.FromFile(filePath);

            vCard vcard = new vCard();
            vcard.Firstname = "Simi";
            vcard.Version = 2.1F;
            vcard.Surname = "Sis";
            vcard.FormattedName = "Sis Simi";
            vcard.Othernames = " ";
            PhoneNumber num1 = new PhoneNumber();
            num1.Number = "07038305040";
            num1.Type = PhoneNumberType.Cell;
            vcard.PhoneNumbers.Add(num1);
            PhoneNumber num2 = new PhoneNumber();
            num2.Number = "09038685791";
            num2.Type = PhoneNumberType.Cell;
            vcard.PhoneNumbers.Add(num2);

            Assert.AreEqual(vcard, vcardCollection[0], "The contacts did not match");
        }
    }
}
