using System;
using System.Text;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 2 cards
    /// </summary>
    internal class V2Serializer : Serializer
    {
        /// <summary>
        /// Converts the vCard properties to a string
        /// </summary>
        /// <param name="vcard">The vcard object to be serialized</param>
        /// <returns>A string representing the vcard properties</returns>
        public  string Serialize(vCard vcard)
        {
            base.SerializeCardStart();
            base.SerializeSharedProperties(vcard);
            foreach (PhoneNumber phoneNumber in vcard.PhoneNumbers)
            {
                StringBuilder.Append(Environment.NewLine);
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    StringBuilder.Append("TEL:" + phoneNumber.Number);
                }
                else if (phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    StringBuilder.Append("TEL;MAIN-NUMBER:" + phoneNumber.Number);
                }
                else
                {
                    StringBuilder.Append("TEL;" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number);
                }
            }

            foreach (EmailAddress email in vcard.EmailAddresses)
            {
                StringBuilder.Append(Environment.NewLine);
                if (email.Type == EmailType.None)
                {
                    StringBuilder.Append("EMAIL:" + email.Email.Address);
                }
                else
                {
                    StringBuilder.Append("EMAIL;" + email.Type.ToString().ToUpper() + ":" + email.Email.Address);
                }
            }

            foreach (Address address in vcard.Addresses)
            {
                StringBuilder.Append(Environment.NewLine);
                if (address.Type == AddressType.None)
                {
                    StringBuilder.Append("ADR:" + address.Location);
                }
                else
                {
                    StringBuilder.Append("ADR;" + address.Type.ToString().ToUpper() + ":" + address.Location);
                }
            }

            foreach (Photo photo in vcard.Pictures)
            {
                StringBuilder.Append(Environment.NewLine);
                StringBuilder.Append("PHOTO;" + photo.Encoding);
                if (photo.Type == PhotoType.URL)
                {
                    StringBuilder.Append(":" + photo.PhotoURL);
                }
                else if (photo.Type == PhotoType.Image)
                {
                    StringBuilder.Append(";ENCODING=BASE64:" + photo.ToBase64String());
                }
            }

            foreach (Expertise expertise in vcard.Expertises)
            {
                StringBuilder.Append(Environment.NewLine);
                StringBuilder.Append("EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area);
            }

            foreach (Hobby hobby in vcard.Hobbies)
            {
                StringBuilder.Append(Environment.NewLine);
                StringBuilder.Append("HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity);
            }

            foreach (Interest interest in vcard.Interests)
            {
                StringBuilder.Append(Environment.NewLine);
                StringBuilder.Append("INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity);
            }
SerializeCardEnd();
            return StringBuilder.ToString();
        }
    }
}
