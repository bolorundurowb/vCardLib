﻿// Created by Bolorunduro Winner-Timothy on  12/1/2016 at 6:27 PM

using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Helpers;

namespace vCardLib.Tests.CollectionTests
{
	[TestFixture]
	public class vCardCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				var vcard = new vCard();
				var vcardCollection = new vCardCollection();
				vcardCollection.Add(vcard);
				vcardCollection[0] = vcard;
				vcard = vcardCollection[0];
				vcardCollection.Remove(vcard);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			var vcardCollection = new vCardCollection();
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
			
			var vcardCollection = new vCardCollection();
		    var vcard = new vCard {Version = VcardVersion.V2};
		    vcardCollection.Add(vcard);
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", VcardVersion.V2);
			});
			
			vcard.Version = VcardVersion.V3;
			vcardCollection.Add(vcard);
			
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", VcardVersion.V3, WriteOptions.Overwrite);
			});
			Assert.Throws<InvalidOperationException>(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", VcardVersion.V2);
			});
			//
			vcard.Version = VcardVersion.V4;
			vcardCollection.Add(vcard);
			Assert.Throws<NotImplementedException>(delegate
			{
				vcardCollection.Save("vcardcollection1.vcf", VcardVersion.V4, WriteOptions.Overwrite);
			});
		}

		[Test]
		public void SaveCollectionsWithVersions()
		{
			if (File.Exists("vcardcollection2.vcf"))
				File.Delete("vcardcollection2.vcf");
			
			var vcardCollection = new vCardCollection();
		    var vcard = new vCard {Version = VcardVersion.V2};
			vcardCollection.Add(vcard);
			
		    Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", VcardVersion.V2);
			});
			Assert.Throws<InvalidOperationException>(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", VcardVersion.V2);
			});
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", VcardVersion.V3, WriteOptions.Overwrite);
			});
			Assert.DoesNotThrow(delegate
			{
				vcardCollection.Save("vcardcollection2.vcf", VcardVersion.V3, WriteOptions.Overwrite, Encoding.ASCII);
			});
//			Assert.Throws<NotImplementedException>(delegate
//			{
//				vcardCollection.Save("vcardcollection2.vcf", Version.V4, WriteOptions.Overwrite);
//			});
		}
	}
}
