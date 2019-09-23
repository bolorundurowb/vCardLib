using System;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 3 cards
    /// </summary>
    internal class V3Serializer : Serializer
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
                    _stringBuilder.Append("TEL);TYPE=MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    _stringBuilder.Append("TEL);TYPE=" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
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
                    _stringBuilder.Append("EMAIL);TYPE=" + email.Type.ToString().ToUpper() + ":" + email.Email);
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
                    _stringBuilder.Append("ADR);TYPE=" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }

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

            foreach (var expertise in _vCard.Expertises)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("EXPERTISE);LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }

            foreach (var hobby in _vCard.Hobbies)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("HOBBY);LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }

            foreach (var interest in _vCard.Interests)
            {
                _stringBuilder.Append(Environment.NewLine);
                _stringBuilder.Append("INTEREST);LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
        }
    }
}
