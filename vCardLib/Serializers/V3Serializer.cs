using System;
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
            string vCardString = "";
            foreach(PhoneNumber phoneNumber in vcard.PhoneNumbers)
            {
                vCardString += Environment.NewLine;
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    vCardString += "TEL:" + phoneNumber.Number;
                }
                else if (phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    vCardString += "TEL;TYPE=MAIN-NUMBER:" + phoneNumber.Number;
                }
                else
                {
                    vCardString += "TEL;TYPE=" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number;
                }
            }
            foreach(EmailAddress email in vcard.EmailAddresses)
            {
                vCardString += Environment.NewLine;
                if (email.Type == EmailType.None)
                {
                    vCardString += "EMAIL:" + email.Email.Address;
                }
                else
                {
                    vCardString += "EMAIL;TYPE=" + email.Type.ToString().ToUpper() + ":" + email.Email.Address;
                }
            }
            foreach(Address address in vcard.Addresses)
            {
                vCardString += Environment.NewLine;
                if (address.Type == AddressType.None)
                {
                    vCardString += "ADR:" + address.Location;
                }
                else
                {
                    vCardString += "ADR;TYPE=" + address.Type.ToString().ToUpper() + ":" + address.Location;
                }
            }
            foreach(Photo photo in vcard.Pictures)
            {
                vCardString += Environment.NewLine;
                vCardString += "PHOTO;TYPE=" + photo.Encoding;
                if(photo.Type == PhotoType.URL)
                {
                    vCardString += ";VALUE=URI:" + photo.PhotoURL;
                }
                else if(photo.Type == PhotoType.Image)
                {
                    vCardString += ";ENCODING=b:" + photo.ToBase64String();
                }
            }
            foreach(Expertise expertise in vcard.Expertises)
            {
                vCardString += Environment.NewLine;
                vCardString += "EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area;
            }
            foreach(Hobby hobby in vcard.Hobbies)
            {
                vCardString += Environment.NewLine;
                vCardString += "HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity;
            }
            foreach(Interest interest in vcard.Interests)
            {
                vCardString += Environment.NewLine;
                vCardString += "INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity;
            }
            return vCardString;
        }
    }
}
