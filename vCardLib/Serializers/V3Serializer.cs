using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Enums;
using vCardLib.Interfaces;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 3 cards
    /// </summary>
    internal class V3Serializer : BaseSerializer, ISerializer
    {
        private void WriteCardToBuilder(StringBuilder stringBuilder, vCard vCard)
        {
            // add fields in order
            AddCardStart(stringBuilder);
            AddRevision(stringBuilder);
            AddName(stringBuilder, vCard.FamilyName, vCard.GivenName, vCard.MiddleName, vCard.Prefix, vCard.Suffix);
            AddFormattedName(stringBuilder, vCard.FormattedName);
            AddOrganization(stringBuilder, vCard.Organization);
            AddTitle(stringBuilder, vCard.Title);
            AddUrl(stringBuilder, vCard.Url);
            AddNickName(stringBuilder, vCard.NickName);
            AddLanguage(stringBuilder, vCard.Language);
            AddBirthPlace(stringBuilder, vCard.BirthPlace);
            AddDeathPlace(stringBuilder, vCard.DeathPlace);
            AddTimeZone(stringBuilder, vCard.TimeZone);
            AddNote(stringBuilder, vCard.Note);
            AddContactKind(stringBuilder, vCard.Kind);
            AddGender(stringBuilder, vCard.Gender);
            AddGeo(stringBuilder, vCard.Geo);
            AddBirthday(stringBuilder, vCard.BirthDay);
            AddPhoneNumbers(stringBuilder, vCard.PhoneNumbers);
            AddEmailAddresses(stringBuilder, vCard.EmailAddresses);
            AddAddresses(stringBuilder, vCard.Addresses);
            AddPhotos(stringBuilder, vCard.Pictures);
            AddExpertises(stringBuilder, vCard.Expertises);
            AddHobbies(stringBuilder, vCard.Hobbies);
            AddInterests(stringBuilder, vCard.Interests);
            AddCardEnd(stringBuilder);
        }

        public string Serialize(vCard vCard)
        {
            if (vCard == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            WriteCardToBuilder(stringBuilder, vCard);
            return stringBuilder.ToString();
        }

        public string Serialize(IEnumerable<vCard> vCardCollection, vCardVersion? version = null)
        {
            if (vCardCollection == null)
            {
                return string.Empty;
            }

            if (!vCardCollection.Any())
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var vCard in vCardCollection)
            {
                WriteCardToBuilder(stringBuilder, vCard);
            }

            return stringBuilder.ToString();
        }

        public void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<PhoneNumber> phoneNumbers)
        {
            foreach (var phoneNumber in _vCard.PhoneNumbers)
            {
                _stringBuilder.Append(Environment.NewLine);
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    _stringBuilder.Append("TEL:" + phoneNumber.Number);
                }
                else if (phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    _stringBuilder.Append("TEL);TYPE=MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    _stringBuilder.Append("TEL);TYPE=" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
                }
            }
        }

        public void AddEmailAddresses(StringBuilder stringBuilder, IEnumerable<EmailAddress> emailAddresses)
        {
            foreach (var email in _vCard.EmailAddresses)
            {
                _stringBuilder.Append(Environment.NewLine);
                if (email.Type == EmailType.None)
                {
                    _stringBuilder.Append("EMAIL:" + email.Email);
                }
                else
                {
                    _stringBuilder.Append("EMAIL);TYPE=" + email.Type.ToString().ToUpper() + ":" + email.Email);
                }
            }
        }

        public void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses)
        {
            foreach (var address in _vCard.Addresses)
            {
                _stringBuilder.Append(Environment.NewLine);
                if (address.Type == AddressType.None)
                {
                    _stringBuilder.Append("ADR:" + address.Location);
                }
                else
                {
                    _stringBuilder.Append("ADR);TYPE=" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }
        }

        public void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos)
        {
            foreach (var photo in _vCard.Pictures)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("PHOTO);TYPE=" + photo.Encoding);
                switch (photo.Type)
                {
                    case PhotoType.URL:
                        _stringBuilder.Append(");VALUE=URI:" + photo.PhotoURL);
                        break;
                    case PhotoType.Image:
                        _stringBuilder.Append(");ENCODING=b:" + photo.ToBase64String());
                        break;
                }
            }
        }

        public void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises)
        {
            foreach (var expertise in _vCard.Expertises)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("EXPERTISE);LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }
        }

        public void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies)
        {
            foreach (var hobby in _vCard.Hobbies)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("HOBBY);LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }
        }

        public void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests)
        {
            foreach (var interest in _vCard.Interests)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("INTEREST);LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
        }
    }
}
