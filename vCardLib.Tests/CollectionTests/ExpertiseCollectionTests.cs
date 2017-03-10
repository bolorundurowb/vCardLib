// Created by Bolorunduro Winner-Timothy on  11/18/2016 at 10:59 AM

using System;
using NUnit.Framework;
using vCardLib.Collections;
using vCardLib.Models;

namespace vCardLib.Tests.CollectionTests
{
	[TestFixture]
	public class ExpertiseCollectionTests
	{
		[Test]
		public void InsertAndRemove()
		{
			Assert.DoesNotThrow(delegate
			{
				Expertise expertise = new Expertise();
				ExpertiseCollection expertiseCollection = new ExpertiseCollection();
				expertiseCollection.Add(expertise);
				expertise = expertiseCollection[0];
				expertiseCollection[0] = expertise;
				expertiseCollection.Remove(expertise);
			});
		}

		[Test]
		public void InsertAndRemoveNonExistentIndices()
		{
			ExpertiseCollection expertiseCollection = new ExpertiseCollection();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				var expertise_ = expertiseCollection[0];
			});
			var expertise = new Expertise();
			Assert.Throws<IndexOutOfRangeException>(delegate
			{
				expertiseCollection[0] = expertise;
			});
		}
	}
}
