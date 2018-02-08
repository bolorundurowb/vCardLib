using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests.CollectionTests
{
	[TestFixture]
	public class AddressCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				var address = new Address();
				var addressCollection = new AddressCollection();
				addressCollection.Add(address);
				address = addressCollection[0];
				addressCollection[0] = address;
				addressCollection.Remove(address);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			var addressCollection = new AddressCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				var address_ = addressCollection[0];
			});
			var address = new Address();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				addressCollection[0] = address;
			});
		}
	}
}
