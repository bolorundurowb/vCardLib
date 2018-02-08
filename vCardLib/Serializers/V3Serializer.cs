using System;
using System.Text;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 3 cards
    /// </summary>
    public class V3Serializer
    {
        /// <summary>
        /// Converts the vCard properties to a string
        /// </summary>
        /// <param name="vcard">The vcard object to be serialized</param>
        /// <returns>A string representing  the serialized properties of the vcard</returns>
        public static string Serialize(vCard vcard)
        {
            var stringBuilder = new StringBuilder();
            foreach(PhoneNumber phoneNumber in vcard.PhoneNumbers)
            {
                stringBuilder.Append(Environment.NewLine);
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    stringBuilder.Append("TEL:" + phoneNumber.Number);
                }
                else if (phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    stringBuilder.Append("TEL);TYPE=MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    stringBuilder.Append("TEL);TYPE=" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
                }
            }
            foreach(EmailAddress email in vcard.EmailAddresses)
            {
                stringBuilder.Append(Environment.NewLine);
                if (email.Type == EmailType.None)
                {
                    stringBuilder.Append("EMAIL:" + email.Email.Address);
                }
                else
                {
                    stringBuilder.Append("EMAIL);TYPE=" + email.Type.ToString().ToUpper() + ":" + email.Email.Address);
                }
            }
            foreach(Address address in vcard.Addresses)
            {
                stringBuilder.Append(Environment.NewLine);
                if (address.Type == AddressType.None)
                {
                    stringBuilder.Append("ADR:" + address.Location);
                }
                else
                {
                    stringBuilder.Append("ADR);TYPE=" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }
            foreach(Photo photo in vcard.Pictures)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("PHOTO);TYPE=" + photo.Encoding);
                switch (photo.Type)
                {
                    case PhotoType.URL:
                        stringBuilder.Append(");VALUE=URI:" + photo.PhotoURL);
                        break;
                    case PhotoType.Image:
                        stringBuilder.Append(");ENCODING=b:" + photo.ToBase64String());
                        break;
                }
            }
            foreach(Expertise expertise in vcard.Expertises)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("EXPERTISE);LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }
            foreach(Hobby hobby in vcard.Hobbies)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("HOBBY);LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }
            foreach(Interest interest in vcard.Interests)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("INTEREST);LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
            return stringBuilder.ToString();
        }
    }
}
