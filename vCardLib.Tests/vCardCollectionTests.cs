// Created by Bolorunduro Winner-Timothy on  12/1/2016 at 6:27 PM
using System;
using System.IO;
using NUnit.Framework;

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
			vcard.Version = 3.0f;
			vcardCollection.Add(vcard);
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf");
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
	}
}
