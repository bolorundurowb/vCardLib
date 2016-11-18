using NUnit.Framework;

namespace vCardLib.Tests
{
	[TestFixture]
	public class EmailAddressTests
	{
		[Test]
		public void EmailAddressIsStable()
		{
			Assert.DoesNotThrow(delegate
			{
				var emailAddress = new EmailAddress();
				emailAddress.Email = new System.Net.Mail.MailAddress("test@test.org");
				emailAddress.Type = EmailType.AOL;
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
