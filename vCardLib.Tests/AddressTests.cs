using NUnit.Framework;
using vCardLib.Models;

namespace vCardLib.Tests
{
	[TestFixture]
	public class AddressTests
	{
		[Test]
		public void AddressIsStable()
		{
			Assert.DoesNotThrow(delegate {
				Address address = new Address();
				address.Location = "Yaba Street";
				address.Type = AddressType.Domestic;
				address.Type = AddressType.Home;
				address.Type = AddressType.International;
				address.Type = AddressType.None;
				address.Type = AddressType.Parcel;
				address.Type = AddressType.Postal;
				address.Type = AddressType.Work;
			});
		}
	}
}
