// Created by Bolorunduro Winner-Timothy on  12/1/2016 at 6:27 PM
using System;
using System.IO;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Helpers;

namespace vCardLib.Tests
{
	[TestFixture]
	public class vCardCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				vCard vcard = new vCard();
				vCardCollection vcardCollection = new vCardCollection();
				vcardCollection.Add(vcard);
				vcardCollection[0] = vcard;
				vcard = vcardCollection[0];
				vcardCollection.Remove(vcard);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			vCardCollection vcardCollection = new vCardCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				var vcard_ = vcardCollection[0];
			});
			var vcard = new vCard();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				vcardCollection[0] = vcard;
			});
		}

		[Test]
		public void SaveCollections()
		{
			if (File.Exists("vcardcollection1.vcf"))
				File.Delete("vcardcollection1.vcf");
			vCardCollection vcardCollection = new vCardCollection();
			vCard vcard = new vCard();
			vcard.Version = 2.1f;
			vcardCollection.Add(vcard);
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf");
			});
			vcard.Version = 3.0f;
			vcardCollection.Add(vcard);
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", WriteOptions.Overwrite);
			});
			Assert.Throws<InvalidOperationException>(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf");
			});
			//
			vcard.Version = 4.0f;
			vcardCollection.Add(vcard);
			Assert.Throws<NotImplementedException>(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", WriteOptions.Overwrite);
			});
			//
			vcard.Version = 5.0f;
			vcardCollection.Add(vcard);
			Assert.Throws<ArgumentException>(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", WriteOptions.Overwrite);
			});
		}

		[Test]
		public void SaveCollectionsWithVersions()
		{
			if (File.Exists("vcardcollection2.vcf"))
				File.Delete("vcardcollection2.vcf");
			vCardCollection vcardCollection = new vCardCollection();
			vCard vcard = new vCard();
			vcard.Version = 2.1f;
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", 2.1f, WriteOptions.ThrowError);
			});
			Assert.Throws<InvalidOperationException>(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", 2.1f);
			});
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", 3.0f, WriteOptions.Overwrite);
			});
			Assert.Throws<NotImplementedException>(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", 4.0f, WriteOptions.Overwrite);
			});
			Assert.Throws<ArgumentException>(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", 5.4f, WriteOptions.Overwrite);
			});
		}
	}
}
