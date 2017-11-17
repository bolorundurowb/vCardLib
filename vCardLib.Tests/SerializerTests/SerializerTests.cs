﻿using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Helpers;
using vCardLib.Serializers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Tests.SerializerTests
{
	[TestFixture]
	public class SerializerTests
	{
		string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		[Test]
		public void SerializeVcardTest()
		{
			string filePath = Path.Combine(assemblyFolder, "v3.vcf");
			vCard vcard = null;
			Assert.Throws<InvalidOperationException>(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V3);
			});
			Assert.Throws<ArgumentNullException>(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V3, WriteOptions.Overwrite);
			});

			vcard = new vCard();
			vcard.Addresses = new AddressCollection();
			vcard.BirthDay = new DateTime();
			vcard.BirthPlace = "Mississipi";
			vcard.DeathPlace = "Washington";
			vcard.EmailAddresses = new EmailAddressCollection();
			vcard.Expertises = new ExpertiseCollection();
			vcard.FamilyName = "Gump";
			vcard.GivenName = "Forrest";
			vcard.MiddleName = "Johnson";
			vcard.Prefix = "HRH";
			vcard.Suffix = "PhD";
			vcard.Gender = GenderType.Female;
			vcard.Hobbies = new HobbyCollection();
			vcard.Interests = new InterestCollection();
			vcard.Kind = ContactType.Application;
			vcard.Language = "English";
			vcard.NickName = "Gumpy";
			vcard.Organization = "Google";
			vcard.PhoneNumbers = new PhoneNumberCollection();
			vcard.Pictures = new PhotoCollection();
			vcard.TimeZone = "GMT+1";
			vcard.Title = "Mr";
			vcard.Url = "http://google.com";
			vcard.Note = "Hello World";
			vcard.Version = Version.V2;

			filePath = Path.Combine(assemblyFolder, "testV2.vcf");
			if ( File.Exists( filePath ) ) File.Delete( filePath );
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V2);
			});
			FileAssert.Exists(filePath);

			filePath = Path.Combine(assemblyFolder, "testV3.vcf");
			if ( File.Exists( filePath ) ) File.Delete( filePath );
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V3);
			});
			FileAssert.Exists(filePath);

			filePath = Path.Combine(assemblyFolder, "testV4.vcf");
			Assert.Throws<NotImplementedException>(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V4);
			});
		}

		[Test]
		public void SerializeVcardErrorTest()
		{
			string filePath = null;
			vCard vcard = new vCard();
			filePath = null;
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V2);
			});

			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V3);
			});

			Assert.Throws<NotImplementedException>(delegate
			{
				Serializer.Serialize(vcard, filePath, Version.V4);
			});
		}

		[Test]
		public void SerializeVcardCollectionTest()
		{
			string filePath = Path.Combine(assemblyFolder, "invalid.vcf");
			vCardCollection vcardCollection = null;
			Assert.Throws<InvalidOperationException>(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V3);
			});
			Assert.Throws<ArgumentNullException>(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V3, WriteOptions.Overwrite);
			});

			vcardCollection = new vCardCollection();
			filePath = Path.Combine(assemblyFolder, "testV2collection.vcf");
			if ( File.Exists( filePath ) ) File.Delete( filePath );
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V2);
			});
			FileAssert.Exists(filePath);

			filePath = Path.Combine(assemblyFolder, "testV3collection.vcf");
			if ( File.Exists( filePath ) ) File.Delete( filePath );
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V3);
			});
			FileAssert.Exists(filePath);

			filePath = Path.Combine(assemblyFolder, "testV4collection.vcf");
			vcardCollection.Add(new vCard());
			Assert.Throws<NotImplementedException>(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V4);
			});
		}

		[Test]
		public void SerializeVcardCollectionErrorTest()
		{
			string filePath = null;
			vCardCollection vcardCollection  = new vCardCollection();
			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V2);
			});

			Assert.DoesNotThrow(delegate
			{
				Serializer.Serialize(vcardCollection, filePath, Version.V3);
			});
		}
	}
}