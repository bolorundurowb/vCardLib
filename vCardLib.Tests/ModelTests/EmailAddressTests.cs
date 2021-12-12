using NUnit.Framework;
using vCardLib.Enums;
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
                    Email = "test@test.org",
                    Type = EmailAddressType.AOL
                };
                emailAddress.Type = EmailAddressType.Applelink;
                emailAddress.Type = EmailAddressType.Home;
                emailAddress.Type = EmailAddressType.IBMMail;
                emailAddress.Type = EmailAddressType.Internet;
                emailAddress.Type = EmailAddressType.None;
                emailAddress.Type = EmailAddressType.Work;
            });
        }
    }
}
