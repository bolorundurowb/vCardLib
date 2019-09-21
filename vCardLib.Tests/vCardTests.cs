using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using vCardLib.Collections;
using vCardLib.Deserializers;
using vCardLib.Helpers;
using vCardLib.Models;

namespace vCardLib.Tests
{
    [TestFixture]
	public class vCardTests
	{
		readonly vCard _vcard = new vCard();
		
		[Test]
		public void GenerateValidVcard()
		{
			Assert.DoesNotThrow(delegate
			{
				_vcard.Addresses = new AddressCollection();
				_vcard.BirthDay = new DateTime();
				_vcard.BirthPlace = "Mississipi";
				_vcard.DeathPlace = "Washington";
				_vcard.EmailAddresses = new EmailAddressCollection();
				_vcard.Expertises = new ExpertiseCollection();
				_vcard.FamilyName = "Gump";
			    _vcard.GivenName = "Forrest";
			    _vcard.MiddleName = "Johnson";
			    _vcard.Prefix = "HRH";
			    _vcard.Suffix = "PhD";
				_vcard.Gender = GenderType.Female;
				_vcard.Hobbies = new HobbyCollection();
				_vcard.Interests = new InterestCollection();
				_vcard.Kind = ContactType.Application;
				_vcard.Language = "English";
				_vcard.NickName = "Gumpy";
				_vcard.Organization = "Google";
				_vcard.PhoneNumbers = new PhoneNumberCollection();
				_vcard.Pictures = new PhotoCollection();
				_vcard.TimeZone = "GMT+1";
				_vcard.Title = "Mr";
				_vcard.Url = "http://google.com";
			    _vcard.Note = "Hello World";
				_vcard.Geo = new Geo()
				{
					Latitude = -1.345,
					Longitude = +34.67
				};
				_vcard.XSkypeDisplayName = "Forrest J Gump";
				_vcard.XSkypePstnNumber = "23949490044";
				_vcard.Version = VcardVersion.V2;
			});
		}

		[Test]
		public void ReadsCardsWithoutErrors()
		{
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "v2.vcf");
			vCardCollection collection = null;
			Assert.DoesNotThrow(delegate {
				collection = vCard.FromFile(filePath);
			});
			Assert.IsNotNull(collection);
			Assert.IsTrue(collection.Count > 0);
			filePath = Path.Combine(assemblyFolder, "v3.vcf");
			collection = null;
			Assert.DoesNotThrow(delegate {
				collection = vCard.FromFile(filePath);
			});
			Assert.IsNotNull(collection);
			Assert.IsTrue(collection.Count > 0);
		}

		[Test]
		public void DoesNotOverwriteExceptInstructed()
		{
			Assert.IsNotNull(_vcard);
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			Assert.Throws<InvalidOperationException>(delegate {
				_vcard.Save(filePath);
			});
		}

        [Test]
        public void SavesV2CardWithoutErrors()
        {
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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
                email1.Email = new System.Net.Mail.MailAddress("forrestgump@example.com");
                email1.Type = EmailType.None;
                _vcard.EmailAddresses.Add(email1);
				var email2 = new EmailAddress();
				email2.Email = new System.Net.Mail.MailAddress("forrestgump@example.com");
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

				var photo2 = new Photo();
				photo2.Type = PhotoType.Image;
				photo2.Encoding = PhotoEncoding.JPEG;
				photo2.Picture = new System.Drawing.Bitmap(responseStream);
				_vcard.Pictures.Add(photo2);

                var hobby = new Hobby();
                hobby.Activity = "Watching Hobbits";
                hobby.Level = Level.Medium;
                _vcard.Hobbies.Add(hobby);
                var interest = new Interest();
                interest.Activity = "Watching Hobbits";
                interest.Level = Level.Medium;
                _vcard.Interests.Add(interest);
                var expertise = new Expertise();
                expertise.Area = "Watching Hobbits";
                expertise.Level = Level.Medium;
                _vcard.Expertises.Add(expertise);

                _vcard.Save(filePath, WriteOptions.Overwrite, Encoding.BigEndianUnicode);
            });
			Assert.IsTrue(File.Exists(filePath));
        }

        [Test]
        public void SavesV3CardWithoutErrors()
        {
            var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "newv3.vcf");
			Assert.IsNotNull(_vcard);
			Assert.DoesNotThrow(delegate {
				_vcard.Save(filePath, VcardVersion.V3, WriteOptions.Overwrite, Encoding.ASCII);
			});
			Assert.IsTrue(File.Exists(filePath));
        }

		[Test]
		public void SavesV4CardThrowsException()
		{
			Assert.Throws<NotImplementedException> (delegate {
				_vcard.Save ("", VcardVersion.V4, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void Read_Saved_Vcard()
		{
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "newv3.vcf");
			Assert.DoesNotThrow(delegate
			{
				var vcard = Deserializer.FromFile(filePath);
			});
		}

		[Test]
		public void Read_Non_UTF8_Vcard()
		{
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "newv2.vcf");
			Assert.DoesNotThrow(delegate
			{
				var vcard = Deserializer.FromFile(filePath);
			});
		}
    }
}

