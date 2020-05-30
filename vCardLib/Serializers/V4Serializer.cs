using System;
using System.Collections.Generic;
using System.Text;
using vCardLib.Interfaces;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 4 cards
    /// </summary>
    internal class V4Serializer : BaseSerializer, ISerializer
    {
        public string Serialize(vCard vCard)
        {
            throw new NotImplementedException();
        }

        public string Serialize(IEnumerable<vCard> vCardCollection)
        {
            throw new NotImplementedException();
        }

        public void AddVersion(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("VERSION:4.0");
        }

        public void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<PhoneNumber> phoneNumbers)
        {
            throw new NotImplementedException();
        }

        public void AddEmailAddresses(StringBuilder stringBuilder, IEnumerable<EmailAddress> emailAddresses)
        {
            throw new NotImplementedException();
        }

        public void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses)
        {
            throw new NotImplementedException();
        }

        public void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos)
        {
            throw new NotImplementedException();
        }

        public void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises)
        {
            throw new NotImplementedException();
        }

        public void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies)
        {
            throw new NotImplementedException();
        }

        public void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests)
        {
            throw new NotImplementedException();
        }
    }
}
