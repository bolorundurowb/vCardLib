// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:38 AM

using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests.CollectionTests
{
	[TestFixture]
	public class PhoneNumberCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				PhoneNumber phoneNumber = new PhoneNumber();
				PhoneNumberCollection phoneNumberCollection = new PhoneNumberCollection();
				phoneNumberCollection.Add(phoneNumber);
				phoneNumber = phoneNumberCollection[0];
				phoneNumberCollection[0] = phoneNumber;
				phoneNumberCollection.Remove(phoneNumber);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			PhoneNumberCollection phoneNumberCollection = new PhoneNumberCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				var phoneNumber_ = phoneNumberCollection[0];
			});
			var phoneNumber = new PhoneNumber();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				phoneNumberCollection[0] = phoneNumber;
			});
		}
	}
}
