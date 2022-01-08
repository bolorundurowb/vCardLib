using NUnit.Framework;
using vCardLib.Enums;

namespace vCardLib.Tests;

[TestFixture]
public class EnumTests
{
	[Test]
	public void GenderIsStable()
	{
		Assert.DoesNotThrow(delegate {
			var gender = GenderType.Male;
			gender = GenderType.Female;
			gender = GenderType.Other;
			gender = GenderType.None;
		});
	}

	[Test]
	public void ContactsAreStable()
	{
		Assert.DoesNotThrow(delegate {
			var contactType = ContactKind.Application;
			contactType = ContactKind.Device;
			contactType = ContactKind.Group;
			contactType = ContactKind.Individual;
			contactType = ContactKind.Location;
			contactType = ContactKind.Organization;
		});
	}

	[Test]
	public void WriteOptionsAreStable()
	{
		Assert.DoesNotThrow(delegate {
			var writeOption = OverWriteOptions.Proceed;
			writeOption = OverWriteOptions.Throw;
		});
	}
}