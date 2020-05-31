using System;
using System.Collections.Generic;
using System.Text;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 4 cards
    /// </summary>
    internal class v4Serializer : Serializer
    {
        protected override void AddVersion(StringBuilder stringBuilder)
        {
            throw new NotImplementedException();
        }

        protected override void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<PhoneNumber> phoneNumbers)
        {
            throw new NotImplementedException();
        }

        protected override void AddEmailAddresses(StringBuilder stringBuilder, IEnumerable<EmailAddress> emailAddresses)
        {
            throw new NotImplementedException();
        }

        protected override void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses)
        {
            throw new NotImplementedException();
        }

        protected override void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos)
        {
            throw new NotImplementedException();
        }

        protected override void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises)
        {
            throw new NotImplementedException();
        }

        protected override void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies)
        {
            throw new NotImplementedException();
        }

        protected override void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests)
        {
            throw new NotImplementedException();
        }
    }
}
