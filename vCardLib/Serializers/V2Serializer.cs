using System;
using System.Text;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 2 cards
    /// </summary>
    public class V2Serializer : Serializer
    {
        /// <summary>
        /// Converts the vCard properties to a string
        /// </summary>
        /// <param name="vcard">The vcard object to be serialized</param>
        /// <returns>A string representing the vcard properties</returns>
        public static string Serialize(vCard vcard)
        {
            var stringBuilder = new StringBuilder();
            foreach (PhoneNumber phoneNumber in vcard.PhoneNumbers)
            {
                stringBuilder.Append(Environment.NewLine);
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    stringBuilder.Append("TEL:" + phoneNumber.Number);
                }
                else if(phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    stringBuilder.Append("TEL;MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    stringBuilder.Append("TEL;" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
                }
            }
            foreach (EmailAddress email in vcard.EmailAddresses)
            {
                stringBuilder.Append(Environment.NewLine);
                if (email.Type == EmailType.None)
                {
                    stringBuilder.Append("EMAIL:" + email.Email.Address);
                }
                else
                {
                    stringBuilder.Append("EMAIL;" + email.Type.ToString().ToUpper() + ":" + email.Email.Address);
                }
            }
            foreach (Address address in vcard.Addresses)
            {
                stringBuilder.Append(Environment.NewLine);
                if (address.Type == AddressType.None)
                {
                    stringBuilder.Append("ADR:" + address.Location);
                }
                else
                {
                    stringBuilder.Append("ADR;" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }
            foreach (Photo photo in vcard.Pictures)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("PHOTO;" + photo.Encoding);
                if (photo.Type == PhotoType.URL)
                {
                    stringBuilder.Append(":" + photo.PhotoURL);
                }
                else if (photo.Type == PhotoType.Image)
                {
                    stringBuilder.Append(";ENCODING=BASE64:" + photo.ToBase64String());
                }
            }
            foreach (Expertise expertise in vcard.Expertises)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }
            foreach (Hobby hobby in vcard.Hobbies)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }
            foreach (Interest interest in vcard.Interests)
            {
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.Append("INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
            return stringBuilder.ToString();
        }
    }
}
