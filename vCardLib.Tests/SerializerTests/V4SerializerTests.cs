using System;
using NUnit.Framework;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Tests
{
	[TestFixture]
	public class V4SerializerTests
	{
		[Test]
		public void SerializerTest()
		{
			Assert.Throws<NotImplementedException>(
				delegate
				{
					V4Serializer.Serialize(new vCard());
				}
			);
		}
	}
}
