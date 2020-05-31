using NUnit.Framework;
using vCardLib.Enums;

namespace vCardLib.Tests
{
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
				var contactType = ContactType.Application;
				contactType = ContactType.Device;
				contactType = ContactType.Group;
				contactType = ContactType.Individual;
				contactType = ContactType.Location;
				contactType = ContactType.Organization;
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
}
