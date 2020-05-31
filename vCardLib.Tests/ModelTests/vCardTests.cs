using System;
using System.Collections.Generic;
using NUnit.Framework;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.ModelTests
{
    [TestFixture]
    public class vCardTests
    {
        readonly vCard _vcard = new vCard();

        [Test]
        public void GenerateValidVcard()
        {
            Assert.DoesNotThrow(delegate
            {
                _vcard.Addresses = new List<Address>();
                _vcard.BirthDay = new DateTime();
                _vcard.BirthPlace = "Mississipi";
                _vcard.DeathPlace = "Washington";
                _vcard.EmailAddresses = new List<EmailAddress>();
                _vcard.Expertises = new List<Expertise>();
                _vcard.FamilyName = "Gump";
                _vcard.GivenName = "Forrest";
                _vcard.MiddleName = "Johnson";
                _vcard.Prefix = "HRH";
                _vcard.Suffix = "PhD";
                _vcard.Gender = GenderType.Female;
                _vcard.Hobbies = new List<Hobby>();
                _vcard.Interests = new List<Interest>();
                _vcard.Kind = ContactType.Application;
                _vcard.Language = "English";
                _vcard.NickName = "Gumpy";
                _vcard.Organization = "Google";
                _vcard.PhoneNumbers = new List<PhoneNumber>();
                _vcard.Pictures = new List<Photo>();
                _vcard.TimeZone = "GMT+1";
                _vcard.Title = "Mr";
                _vcard.Url = "http://google.com";
                _vcard.Note = "Hello World";
                _vcard.Geo = new Geo
                {
                    Latitude = -1.345,
                    Longitude = +34.67
                };
                _vcard.CustomFields = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("XSkypeDisplayName", "Forrest J Gump"),
                    new KeyValuePair<string, string>("XSkypePstnNumber", "23949490044")
                };
                _vcard.Version = vCardVersion.V2;
            });
        }
    }
}
