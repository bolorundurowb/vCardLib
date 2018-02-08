using NUnit.Framework;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class AddressTests
	{
		[Test]
		public void AddressIsStable()
		{
			Assert.DoesNotThrow(delegate {
			    var address = new Address
			    {
			        Location = "Yaba Street",
			        Type = AddressType.Domestic
			    };
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
