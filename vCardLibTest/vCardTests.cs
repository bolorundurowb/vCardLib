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

            string file = @"VERSION:2.1
N:Gump;Forrest
FN:Forrest Gump
ORG:Bubba Gump Shrimp Co.
TITLE:Shrimp Man
PHOTO;GIF:http://www.example.com/dir_photos/my_photo.gif
TEL;WORK;VOICE:(111) 555-1212
TEL;HOME;VOICE:(404) 555-1212
ADR;WORK:;;100 Waters Edge;Baytown;LA;30314;United States of America
LABEL;WORK;ENCODING=QUOTED-PRINTABLE:100 Waters Edge=0D=0ABaytown, LA 30314=0D=0AUnited States of America
ADR;HOME:;;42 Plantation St.;Baytown;LA;30314;United States of America
LABEL;HOME;ENCODING=QUOTED-PRINTABLE:42 Plantation St.=0D=0ABaytown, LA 30314=0D=0AUnited States of America
EMAIL;PREF;INTERNET:forrestgump@example.com
REV:20080424T195243Z
END:VCARD";
            Stream stream = GenerateStreamFromString(file);
            vCardCollection vcardCollection = vCard.FromStreamReader(new StreamReader(stream));
        }

        [TestMethod]
        public void vCardWithValidFile()
        {
            string file = @"BEGIN:VCARD
VERSION:2.1
N:Gump;Forrest
FN:Forrest Gump
TEL;WORK;VOICE:(111) 555-1212
TEL;HOME;VOICE:(404) 555-1212
ADR;WORK:;;100 Waters Edge;Baytown;LA;30314;United States of America
ADR;HOME:;;42 Plantation St.;Baytown;LA;30314;United States of America
EMAIL;PREF;INTERNET:forrestgump@example.com
END:VCARD";
            Stream stream = GenerateStreamFromString(file);
            vCardCollection vcardCollection = vCard.FromStreamReader(new StreamReader(stream));

            vCard vcard = new vCard();
            vcard.Firstname = "Gump";
            vcard.Version = 2.1F;
            vcard.Surname = "Forrest";
            vcard.FormattedName = "Forrest Gump";
            vcard.Othernames = " ";
            PhoneNumber num1 = new PhoneNumber();
            num1.Number = "(111) 555-1212";
            num1.Type = PhoneNumberType.Work;
            vcard.PhoneNumbers.Add(num1);
            PhoneNumber num2 = new PhoneNumber();
            num2.Number = "(404) 555-1212";
            num2.Type = PhoneNumberType.Home;
            vcard.PhoneNumbers.Add(num2);
            EmailAddress email = new EmailAddress();
            email.Email = new System.Net.Mail.MailAddress("forrestgump@example.com");
            email.Type = EmailType.Internet;
            vcard.EmailAddresses.Add(email);

            Assert.AreEqual(vcard, vcardCollection[0], "The contacts did not match");
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void UnsupportedVcardVersion()
        {
            string file = @"BEGIN:VCARD
VERSION:4.0
END:VCARD";
            Stream stream = GenerateStreamFromString(file);
            vCardCollection vcardCollection = vCard.FromStreamReader(new StreamReader(stream));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VcardWithNullStream()
        {
            StreamReader sr = null;
            vCard.FromStreamReader(sr);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetContactsFromEmptyString()
        {
            string contactString = String.Empty;
            Helper.GetContactsArrayFromString(contactString);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void VcardWithoutVersionTag()
        {
            string file = @"BEGIN:VCARD
EMAIL:test@example.com
END:VCARD";
            Stream stream = GenerateStreamFromString(file);
            vCardCollection vcardCollection = vCard.FromStreamReader(new StreamReader(stream));
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
