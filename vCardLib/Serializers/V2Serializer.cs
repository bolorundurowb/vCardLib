using System;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 2 cards
    /// </summary>
    internal class V2Serializer : Serializer
    {
        protected override void AddVersionedFields()
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
                    _stringBuilder.Append("TEL;MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    _stringBuilder.Append("TEL;" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
                }
            }

            foreach (var email in _vCard.EmailAddresses)
            {
                _stringBuilder.Append(Environment.NewLine);
                if (email.Type == EmailType.None)
                {
                    _stringBuilder.Append("EMAIL:" + email.Email);
                }
                else
                {
                    _stringBuilder.Append("EMAIL;" + email.Type.ToString().ToUpper() + ":" + email.Email);
                }
            }

            foreach (var address in _vCard.Addresses)
            {
                _stringBuilder.Append(Environment.NewLine);
                if (address.Type == AddressType.None)
                {
                    _stringBuilder.Append("ADR:" + address.Location);
                }
                else
                {
                    _stringBuilder.Append("ADR;" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }

            foreach (var photo in _vCard.Pictures)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("PHOTO;" + photo.Encoding);
                if (photo.Type == PhotoType.URL)
                {
                    _stringBuilder.Append(":" + photo.PhotoURL);
                }
                else if (photo.Type == PhotoType.Image)
                {
                    _stringBuilder.Append(";ENCODING=BASE64:" + photo.ToBase64String());
                }
            }

            foreach (var expertise in _vCard.Expertises)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }

            foreach (var hobby in _vCard.Hobbies)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }

            foreach (var interest in _vCard.Interests)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
        }
    }
}
