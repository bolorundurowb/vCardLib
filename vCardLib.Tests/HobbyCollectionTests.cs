// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:18 AM
using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests
{
	[TestFixture]
	public class HobbyCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				Hobby hobby = new Hobby();
				HobbyCollection hobbyCollection = new HobbyCollection();
				hobbyCollection.Add(hobby);
				hobby = hobbyCollection[0];
				hobbyCollection[0] = hobby;
				hobbyCollection.Remove(hobby);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			HobbyCollection hobbyCollection = new HobbyCollection();
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
