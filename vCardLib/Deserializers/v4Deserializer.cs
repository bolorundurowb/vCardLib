using System;
using System.Collections.Generic;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserializers
{
    // ReSharper disable once InconsistentNaming
    public class v4Deserializer : Deserializer
    {
        protected override vCardVersion ParseVersion()
        {
            return vCardVersion.V4;
        }

        protected override List<Address> ParseAddresses(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<PhoneNumber> ParsePhoneNumbers(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<EmailAddress> ParseEmailAddresses(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<Hobby> ParseHobbies(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<Expertise> ParseExpertises(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<Interest> ParseInterests(string[] contactDetails)
        {
            throw new NotImplementedException();
        }

        protected override List<Photo> ParsePhotos(string[] contactDetails)
        {
            throw new NotImplementedException();
        }
    }
}
