// Created by Bolorunduro Winner-Timothy on  11/22/2016 at 5:18 AM

using NUnit.Framework;
using vCardLib.Helpers;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class HobbyTests
	{
		[Test]
		public void HobbyIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
			    Hobby hobby = new Hobby
			    {
			        Activity = "Tennis",
			        Level = Level.High
			    };
			    hobby.Level = Level.Medium;
				hobby.Level = Level.Low;
			});
		}
	}
}
