using NUnit.Framework;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
	[TestFixture]
	public class EmailAddressTests
	{
		[Test]
		public void EmailAddressIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
			    var emailAddress = new EmailAddress
			    {
			        Email = new System.Net.Mail.MailAddress("test@test.org"),
			        Type = EmailType.AOL
			    };
			    emailAddress.Type = EmailType.Applelink;
				emailAddress.Type = EmailType.Home;
				emailAddress.Type = EmailType.IBMMail;
				emailAddress.Type = EmailType.Internet;
				emailAddress.Type = EmailType.None;
				emailAddress.Type = EmailType.Work;
			});
		}
	}
}
