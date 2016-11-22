// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:33 AM
using NUnit.Framework;

namespace vCardLib.Tests
{
	[TestFixture]
	public class InterestTests
	{
		[Test]
		public void InterestIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
				Interest interest = new Interest();
				interest.Activity = "Tennis";
				interest.Level = Level.High;
				interest.Level = Level.Medium;
				interest.Level = Level.Low;
			});
		}
	}
}
