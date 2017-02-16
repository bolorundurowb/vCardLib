using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests
{
	[TestFixture]
	public class EmailAddressCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				EmailAddress emailAddress = new EmailAddress();
				EmailAddressCollection emailCollection = new EmailAddressCollection();
				emailCollection.Add(emailAddress);
				emailCollection[0] = emailAddress;
				emailAddress = emailCollection[0];
				emailCollection.Remove(emailAddress);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			EmailAddressCollection emailCollection = new EmailAddressCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate {
				var email_ = emailCollection[0];
			});
			var email = new EmailAddress();
			Assert.Throws<IndexOutOfRangeException>(delegate {
				emailCollection[0] = email;
			});
		}
	}
}
