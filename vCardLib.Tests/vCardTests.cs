using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;

namespace vCardLib.Tests
{
    [TestFixture]
	public class Test
	{
		[Test]
		public void EmptyOrNullFilePathThrowsArgumentNullException()
		{
			string filePath = null;
			Assert.Throws<ArgumentNullException>(delegate { vCard.FromFile(filePath); });
			filePath = String.Empty;
			Assert.Throws<ArgumentNullException>(delegate { vCard.FromFile(filePath); });
		}

		[Test]
		public void NonExistentFilePathThrowsFileNotFoundException()
		{
			string filePath = @"C:\Test.vcf";
			Assert.Throws<FileNotFoundException>(delegate { vCard.FromFile(filePath); });
		}

		[Test]
		public void InvalidFileThrowsInvalidOperationException()
		{
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			StreamReader streamReader = new StreamReader(filePath);
			Assert.Throws<InvalidOperationException>(delegate { vCard.FromStreamReader(streamReader); });
		}

		[Test]
		public void ValidVcard()
		{
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "valid.vcf");
			vCardCollection vcardCollection = vCard.FromFile(filePath);
			//
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
			//
			Assert.AreEqual(vcardCollection[0], vcard, "The contacts didn't match");
		}

		[Test]
		public void UnsupportedFileThrowsNotImplementedException()
		{
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "unsupported.vcf");
			Assert.Throws<NotImplementedException>(delegate { vCard.FromFile(filePath); });
		}

        [Test]
        public void WrittenVcard2IsValid()
        {
            vCard vcard = new vCard();
            vcard.Version = 2.1F;
            vcard.Surname = "Bolorunduro";
            EmailAddress email = new EmailAddress();
            email.Email = new System.Net.Mail.MailAddress("ogatimo@gmail.com");
            email.Type = EmailType.Internet;
            vcard.EmailAddresses.Add(email);
            PhoneNumber phone = new PhoneNumber();
            phone.Number = "009238992";
            phone.Type = PhoneNumberType.MainNumber;
            vcard.PhoneNumbers.Add(phone);

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "new.vcf");

            vcard.Save(filePath, WriteOptions.Overwrite);
            Assert.IsTrue(File.Exists(filePath));

            Assert.Greater(vCard.FromFile(filePath).Count, 0);

            Assert.AreEqual(vCard.FromFile(filePath)[0].Surname, vcard.Surname);
        }

        [Test]
        public void WrittenVcard3IsValid()
        {
            vCard vcard = new vCard();
            vcard.Version = 3.0F;
            vcard.Surname = "Bolorunduro";
            EmailAddress email = new EmailAddress();
            email.Email = new System.Net.Mail.MailAddress("ogatimo@gmail.com");
            email.Type = EmailType.Internet;
            vcard.EmailAddresses.Add(email);
            PhoneNumber phone = new PhoneNumber();
            phone.Number = "009238992";
            phone.Type = PhoneNumberType.MainNumber;
            vcard.PhoneNumbers.Add(phone);

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "newv3.vcf");

            vcard.Save(filePath, WriteOptions.Overwrite);
            Assert.IsTrue(File.Exists(filePath));

            Assert.Greater(vCard.FromFile(filePath).Count, 0);

            string number = vCard.FromFile(filePath)[0].PhoneNumbers[0].Number;
            Assert.AreEqual(number, vcard.PhoneNumbers[0].Number);
        }

        [Test]
        public void UnsupportedVcardVersionThrowsNotImplementedException()
        {
            vCard vcard = new vCard();
            vcard.Version = 4.0F;
            Assert.Throws<NotImplementedException>(delegate { vcard.Save("temp.vcf"); });
        }

        [Test]
        public void UseAllVcardProperties()
        {
            Assert.DoesNotThrow(delegate
            {
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
                vcard.URL = "http://www.google.com";
                vcard.Organization = "Facebook";
                vcard.Title = "baba nla fuji";
                vcard.NickName = "Pasuma";
                vcard.Kind = ContactType.Individual;
                vcard.Gender = GenderType.Male;
                vcard.Language = "en-US";
                vcard.BirthDay = DateTime.Now;
                vcard.BirthPlace = "Makurdi";
                vcard.DeathPlace = "Takoraddi";
                vcard.TimeZone = "GMT-1";
                Address address = new Address();
                address.Location = "Sabo, Yaba";
                address.Type = AddressType.Work;
                vcard.Addresses.Add(address);
                Photo photo = new Photo();
                photo.Encoding = PhotoEncoding.JPEG;
                photo.PhotoURL = "www.google/images";
                photo.Type = PhotoType.URL;
                vcard.Pictures.Add(photo);
                Hobby hobby = new Hobby();
                hobby.Activity = "Watching Hobbits";
                hobby.Level = Level.Medium;
                vcard.Hobbies.Add(hobby);
                Interest interest = new Interest();
                interest.Activity = "Watching Hobbits";
                interest.Level = Level.Medium;
                vcard.Interests.Add(interest);
                Expertise expertise = new Expertise();
                expertise.Area = "Watching Hobbits";
                expertise.Level = Level.Medium;
                vcard.Expertises.Add(expertise);

                string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string filePath = Path.Combine(assemblyFolder, "newAll.vcf");
                vcard.Save(filePath, WriteOptions.Overwrite);
            });
        }

        [Test]
        public void WrittenVcardCollectionIsValid()
        {
            vCard vcard = new vCard();
            vcard.Version = 2.1F;
            vcard.Surname = "Bolorunduro";
            EmailAddress email = new EmailAddress();
            email.Email = new System.Net.Mail.MailAddress("ogatimo@gmail.com");
            email.Type = EmailType.Internet;
            vcard.EmailAddresses.Add(email);
            PhoneNumber phone = new PhoneNumber();
            phone.Number = "009238992";
            phone.Type = PhoneNumberType.MainNumber;
            vcard.PhoneNumbers.Add(phone);

            vCardCollection collection = new vCardCollection();
            collection.Add(vcard);
            collection.Add(vcard);

            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string filePath = Path.Combine(assemblyFolder, "newCollection.vcf");

            collection.Save(filePath, 3.0F, WriteOptions.Overwrite);
            Assert.IsTrue(File.Exists(filePath));

            Assert.Greater(vCard.FromFile(filePath).Count, 0);

            string number = vCard.FromFile(filePath)[0].PhoneNumbers[0].Number;
            Assert.AreEqual(number, vcard.PhoneNumbers[0].Number);
        }

		[Test]
		public void InvalidVcardVersionThrowsException()
		{
			vCard vcard = new vCard ();
			Assert.Throws<ArgumentException> (delegate {
				vcard.Save ("", 5.2F, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void VersionFourVcardThrowsException()
		{
			vCard vcard = new vCard ();
			Assert.Throws<NotImplementedException> (delegate {
				vcard.Save ("", 4.0F, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void InvalidVcardCollecionVersionThrowsException()
		{
			vCardCollection collection = new vCardCollection ();
			Assert.Throws<ArgumentException> (delegate {
				collection.Save ("", 5.2F, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void VersionFourVcardCollectionThrowsException()
		{
			vCardCollection collection = new vCardCollection ();
			Assert.Throws<NotImplementedException> (delegate {
				collection.Save ("", 4.0F, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void ObjectCollectionsDontThrowExceptions()
		{
			PhoneNumberCollection numberCollection = new PhoneNumberCollection();
			PhoneNumber number = new PhoneNumber();
			numberCollection.Add(number);
			Assert.AreEqual (numberCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(PhoneNumber), numberCollection [0]);
			numberCollection.Remove(number);
			Assert.AreEqual (numberCollection.Count, 0);

			EmailAddressCollection emailCollection = new EmailAddressCollection();
			EmailAddress email = new EmailAddress();
			emailCollection.Add(email);
			Assert.AreEqual (emailCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(EmailAddress), emailCollection [0]);
			emailCollection.Remove(email);
			Assert.AreEqual (emailCollection.Count, 0);

			AddressCollection addressCollection = new AddressCollection();
			Address address = new Address();
			addressCollection.Add(address);
			Assert.AreEqual (addressCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(Address), addressCollection [0]);
			addressCollection.Remove(address);
			Assert.AreEqual (addressCollection.Count, 0);

			HobbyCollection hobbyCollection = new HobbyCollection();
			Hobby hobby = new Hobby();
			hobbyCollection.Add(hobby);
			Assert.AreEqual (hobbyCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(Hobby), hobbyCollection [0]);
			hobbyCollection.Remove(hobby);
			Assert.AreEqual (hobbyCollection.Count, 0);

			InterestCollection interestCollection = new InterestCollection();
			Interest interest = new Interest();
			interestCollection.Add(interest);
			Assert.AreEqual (interestCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(Interest), interestCollection [0]);
			interestCollection.Remove(interest);
			Assert.AreEqual (interestCollection.Count, 0);

			ExpertiseCollection expertiseCollection = new ExpertiseCollection();
			Expertise expertise = new Expertise();
			expertiseCollection.Add(expertise);
			Assert.AreEqual (expertiseCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(Expertise), expertiseCollection [0]);
			expertiseCollection.Remove(expertise);
			Assert.AreEqual (expertiseCollection.Count, 0);

			PhotoCollection photoCollection = new PhotoCollection();
			Photo photo = new Photo();
			photoCollection.Add(photo);
			Assert.AreEqual (photoCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(Photo), photoCollection [0]);
			photoCollection.Remove(photo);
			Assert.AreEqual (photoCollection.Count, 0);

			vCardCollection vcardCollection = new vCardCollection();
			vCard vcard = new vCard();
			vcardCollection.Add(vcard);
			Assert.AreEqual (vcardCollection.Count, 1);
			Assert.IsInstanceOfType (typeof(vCard), vcardCollection [0]);
			vcardCollection.Remove(vcard);
			Assert.AreEqual (vcardCollection.Count, 0);
		}
    }
}

