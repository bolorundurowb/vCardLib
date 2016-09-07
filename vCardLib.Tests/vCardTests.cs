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
	}
}

