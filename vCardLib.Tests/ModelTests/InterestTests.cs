// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:33 AM

using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class InterestTests
	{
		[Test]
		public void InterestIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
			    var interest = new Interest
			    {
			        Activity = "Tennis",
			        Level = Level.High
			    };
			    interest.Level = Level.Medium;
				interest.Level = Level.Low;
			});
		}
	}
}
