// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:18 AM

using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests.CollectionTests
{
	[TestFixture]
	public class HobbyCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				var hobby = new Hobby();
			    var hobbyCollection = new HobbyCollection {hobby};
			    hobby = hobbyCollection[0];
				hobbyCollection[0] = hobby;
				hobbyCollection.Remove(hobby);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			var hobbyCollection = new HobbyCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				var hobby_ = hobbyCollection[0];
			});
			var hobby = new Hobby();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				hobbyCollection[0] = hobby;
			});
		}
	}
}
