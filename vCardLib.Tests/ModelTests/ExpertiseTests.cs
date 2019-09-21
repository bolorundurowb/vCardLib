// Created by Bolorunduro Winner-Timothy on  11/18/2016 at 10:51 AM

using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class ExpertiseTests
	{
		[Test]
		public void ExpertiseIsStable()
		{
			Assert.DoesNotThrow(delegate {
			    var expertise = new Expertise
			    {
			        Area = "Engineering",
			        Level = Level.High
			    };
			    expertise.Level = Level.Low;
				expertise.Level = Level.Medium;
			});
		}
	}
}
