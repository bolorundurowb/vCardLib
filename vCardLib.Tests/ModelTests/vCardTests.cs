using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
    [TestFixture]
    public class vCardTests
    {
        readonly vCard _vcard = new vCard();
        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        [Test]
        public void GenerateValidVcard()
        {
            Assert.DoesNotThrow(delegate
            {
                _vcard.Addresses = new List<Address>();
                _vcard.BirthDay = new DateTime();
                _vcard.BirthPlace = "Mississipi";
                _vcard.DeathPlace = "Washington";
                _vcard.EmailAddresses = new List<EmailAddress>();
                _vcard.Expertises = new List<Expertise>();
                _vcard.FamilyName = "Gump";
                _vcard.GivenName = "Forrest";
                _vcard.MiddleName = "Johnson";
                _vcard.Prefix = "HRH";
                _vcard.Suffix = "PhD";
                _vcard.Gender = GenderType.Female;
                _vcard.Hobbies = new List<Hobby>();
                _vcard.Interests = new List<Interest>();
                _vcard.Kind = ContactType.Application;
                _vcard.Language = "English";
                _vcard.NickName = "Gumpy";
                _vcard.Organization = "Google";
                _vcard.PhoneNumbers = new List<PhoneNumber>();
                _vcard.Pictures = new List<Photo>();
                _vcard.TimeZone = "GMT+1";
                _vcard.Title = "Mr";
                _vcard.Url = "http://google.com";
                _vcard.Note = "Hello World";
                _vcard.Geo = new Geo
                {
                    Latitude = -1.345,
                    Longitude = +34.67
                };
                _vcard.CustomFields = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("XSkypeDisplayName", "Forrest J Gump"),
                    new KeyValuePair<string, string>("XSkypePstnNumber", "23949490044")
                };
                _vcard.Version = vCardVersion.V2;
            });
        }

//        [Test]
//        public void ReadsCardsWithoutErrors()
//        {
//            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
//            var filePath = Path.Combine(assemblyFolder, "v2.vcf");
//            vCardCollection collection = null;
//            Assert.DoesNotThrow(delegate { collection = vCard.FromFile(filePath); });
//            Assert.IsNotNull(collection);
//            Assert.IsTrue(collection.Count > 0);
//            filePath = Path.Combine(assemblyFolder, "v3.vcf");
//            collection = null;
//            Assert.DoesNotThrow(delegate { collection = vCard.FromFile(filePath); });
//            Assert.IsNotNull(collection);
//            Assert.IsTrue(collection.Count > 0);
//        }

        [Test]
        public void DoesNotOverwriteExceptInstructed()
        {
            Assert.IsNotNull(_vcard);
            var filePath = Path.Combine(assemblyFolder, "invalid.vcf");
            Assert.Throws<InvalidOperationException>(delegate { _vcard.Save(filePath, overWriteOptions: OverWriteOptions.Throw); });
        }

        [Test]
        public void SavesV2CardWithoutErrors()
        {
            var filePath = Path.Combine(assemblyFolder, "newv2.vcf");
            Assert.IsNotNull(_vcard);
            Assert.DoesNotThrow(delegate
            {
                _vcard.GivenName = "Forrest";
                _vcard.FamilyName = "Gump";
                var num1 = new PhoneNumber();
                num1.Number = "(111) 555-1212";
                num1.Type = PhoneNumberType.None;
                _vcard.PhoneNumbers.Add(num1);
                var num2 = new PhoneNumber();
                num2.Number = "(404) 555-1212";
                num2.Type = PhoneNumberType.Home;
                _vcard.PhoneNumbers.Add(num2);
                var num3 = new PhoneNumber();
                num3.Number = "(404) 555-1212";
                num3.Type = PhoneNumberType.MainNumber;
                _vcard.PhoneNumbers.Add(num3);
                var email1 = new EmailAddress();
                email1.Email = "forrestgump@example.com";
                email1.Type = EmailType.None;
                _vcard.EmailAddresses.Add(email1);
                var email2 = new EmailAddress();
                email2.Email = "forrestgump@example.com";
                email2.Type = EmailType.Internet;
                _vcard.EmailAddresses.Add(email2);
                var address1 = new Address();
                address1.Location = "Sabo, Yaba";
                address1.Type = AddressType.None;
                _vcard.Addresses.Add(address1);
                var address2 = new Address();
                address2.Location = "Sabo, Yaba";
                address2.Type = AddressType.Work;
                _vcard.Addresses.Add(address2);
                var photo1 = new Photo();
                photo1.Encoding = PhotoEncoding.JPEG;
                photo1.PhotoURL = "www.google/images";
                photo1.Type = PhotoType.URL;
                _vcard.Pictures.Add(photo1);

                var request = System.Net.WebRequest.Create("https://jpeg.org/images/jpeg-logo-plain.png");
                var response = request.GetResponse();
                var responseStream = response.GetResponseStream();

                var photo2 = new Photo {Type = PhotoType.Image, Encoding = PhotoEncoding.JPEG};
                using (var memoryStream = new MemoryStream())
                {
                    responseStream.CopyTo(memoryStream);
                    photo2.Picture = memoryStream.ToArray();
                }
                _vcard.Pictures.Add(photo2);

                var hobby = new Hobby {Activity = "Watching Hobbits", Level = Level.Medium};
                _vcard.Hobbies.Add(hobby);
                var interest = new Interest {Activity = "Watching Hobbits", Level = Level.Medium};
                _vcard.Interests.Add(interest);
                var expertise = new Expertise {Area = "Watching Hobbits", Level = Level.Medium};
                _vcard.Expertises.Add(expertise);

                _vcard.Save(filePath, Encoding.Unicode);
            });
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void SavesV3CardWithoutErrors()
        {
            var filePath = Path.Combine(assemblyFolder, "newv3.vcf");
            Assert.IsNotNull(_vcard);
            Assert.DoesNotThrow(delegate { _vcard.Save(filePath, Encoding.ASCII, vCardVersion.V3); });
            Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void SavesV4CardThrowsException()
        {
            Assert.Throws<NotImplementedException>(delegate { _vcard.Save("", version: vCardVersion.V4); });
        }
    }
}
