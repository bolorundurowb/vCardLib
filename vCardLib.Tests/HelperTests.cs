// Created by Bolorunduro Winner-Timothy on  11/25/2016 at 7:43 AM
using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
namespace vCardLib.Tests
{
	[TestFixture]
	public class HelperTests
	{
		[Test]
		public void GetStreamReaderChecksValidity()
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
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			StreamReader streamReader = Helper.GetStreamReaderFromFile(filePath);
			Assert.IsNotNull(streamReader);
		}

		[Test]
		public void GetStringFromStreamReaderChecksValidity()
		{
			StreamReader streamReader = null;
			Assert.Throws<ArgumentNullException>(delegate {
				Helper.GetStringFromStreamReader(streamReader);
			});
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			streamReader = Helper.GetStreamReaderFromFile(filePath);
			string vcardString = null;
			Assert.DoesNotThrow(delegate {
				vcardString = Helper.GetStringFromStreamReader(streamReader);
			});
			Assert.IsNotNull(vcardString);
			Assert.IsNotEmpty(vcardString);
		}

		[Test]
		public void GetContactsArrayFromStringChecksValidity()
		{
			string contactsString = "   ";
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
		public void GetContactDetailsArrayFromStringIsStable()
		{
			string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string filePath = Path.Combine(assemblyFolder, "valid.vcf");
			var streamReader = Helper.GetStreamReaderFromFile(filePath);
			var vcardString = Helper.GetStringFromStreamReader(streamReader);
			var contacts = Helper.GetContactsArrayFromString(vcardString);
			string[] contactDetails = null;
			Assert.DoesNotThrow(delegate
			{
				contactDetails = Helper.GetContactDetailsArrayFromString(contacts[0]);
			});
			Assert.IsNotNull(contactDetails);
			Assert.IsTrue(contactDetails.Length > 0);
		}
	}
}
