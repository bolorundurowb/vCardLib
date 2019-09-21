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
		[Test]
		public void GetStreamReaderTest()
		{
			string filePath = null;
			Assert.Throws<ArgumentNullException>(delegate { 
				Helper.GetStreamReaderFromFile(filePath);
			});
			filePath = String.Empty;
			Assert.Throws<ArgumentNullException>(delegate { 
				Helper.GetStreamReaderFromFile(filePath); 
			});
			filePath = @"C:\Test.vcf";
			Assert.Throws<FileNotFoundException>(delegate {
				Helper.GetStreamReaderFromFile(filePath); 
			});
			
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			var streamReader = Helper.GetStreamReaderFromFile(filePath);
			Assert.IsNotNull(streamReader);
		}

		[Test]
		public void GetStringFromStreamReaderTest()
		{
			StreamReader streamReader = null;
			Assert.Throws<ArgumentNullException>(delegate {
				Helper.GetStringFromStreamReader(streamReader);
			});
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			streamReader = Helper.GetStreamReaderFromFile(filePath);
			string vcardString = null;
			Assert.DoesNotThrow(delegate {
				vcardString = Helper.GetStringFromStreamReader(streamReader);
			});
			Assert.IsNotNull(vcardString);
			Assert.IsNotEmpty(vcardString);
		}

		[Test]
		public void GetContactsArrayFromStringTest()
		{
			var contactsString = "   ";
			Assert.Throws<ArgumentException>(delegate {
				Helper.GetContactsArrayFromString(contactsString);
			});
			contactsString = "hello!";
			Assert.Throws<InvalidOperationException>(delegate {
				Helper.GetContactsArrayFromString(contactsString);
			});
			contactsString = "BEGIN:VCARD\r\nEND:VCARD";
			string[] contacts = null;
			Assert.DoesNotThrow(delegate {
				contacts = Helper.GetContactsArrayFromString(contactsString);
			});
			Assert.IsNotNull(contacts);
			Assert.IsTrue(contacts.Length > 0);
		}

		[Test]
		public void GetContactDetailsArrayFromStringTest()
		{
			var assemblyFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var filePath = Path.Combine(assemblyFolder, "v2.vcf");
			var streamReader = Helper.GetStreamReaderFromFile(filePath);
			var vcardString = Helper.GetStringFromStreamReader(streamReader);
			var contacts = Helper.GetContactsArrayFromString(vcardString);
			string[] contactDetails = null;
			Assert.DoesNotThrow(delegate {
				contactDetails = Helper.GetContactDetailsArrayFromString(contacts[0]);
			});
			Assert.IsNotNull(contactDetails);
			Assert.IsTrue(contactDetails.Length > 0);
		}

		[Test]
		public void GetImageFromBase64String()
		{
			string base64String = null;
			var image = Helper.GetImageFromBase64String(base64String);
			Assert.IsNull(image);
			base64String = @"R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";
			image = Helper.GetImageFromBase64String(base64String);
			//Assert.IsNotNull(image);
		}
	}
}
