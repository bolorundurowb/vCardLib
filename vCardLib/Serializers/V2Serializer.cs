using System;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    public class V2Serializer
    {
        internal static string Serialize(vCard vcard)
        {
            string vCardString = "";
            vCardString += "BEGIN:VCARD" + Environment.NewLine;
            vCardString += "VERSION:2.1" + Environment.NewLine;
            vCardString += "N:" + vcard.FamilyName + ";" + vcard.GivenName + ";" + vcard.MiddleName + ";" + vcard.Prefix + ";" + vcard.Suffix + Environment.NewLine;
            vCardString += "FN:" + vcard.FormattedName + Environment.NewLine;
            vCardString += "ORG:" + vcard.Organization + Environment.NewLine;
            vCardString += "TITLE:" + vcard.Title + Environment.NewLine;
            vCardString += "URL:" + vcard.Url + Environment.NewLine;
            vCardString += "NICKNAME:" + vcard.NickName + Environment.NewLine;
            vCardString += "KIND:" + vcard.Kind.ToString().ToUpper() + Environment.NewLine;
            vCardString += "GENDER:" + vcard.Gender + Environment.NewLine;
            vCardString += "LANG:" + vcard.Language + Environment.NewLine;
            vCardString += "BIRTHPLACE:" + vcard.BirthPlace + Environment.NewLine;
            vCardString += "DEATHPLACE:" + vcard.DeathPlace + Environment.NewLine;
            vCardString += "TZ:" + vcard.TimeZone + Environment.NewLine;
            vCardString += "X-SKYPE-DISPLAYNAME:" + vcard.XSkypeDisplayName + Environment.NewLine;
            vCardString += "X-SKYPE-PSTNNUMBER:" + vcard.XSkypePstnNumber + Environment.NewLine;
            if (vcard.Geo != null)
            {
                vCardString += "GEO:" + vcard.Geo.Longitude + ";" + vcard.Geo.Latitude;
            }
            if (vcard.BirthDay != null)
            {
                var birthDay = (DateTime) vcard.BirthDay;
                vCardString += "BDAY:" + birthDay.Year + birthDay.Month.ToString("00") + birthDay.Day.ToString("00");
            }
            foreach (PhoneNumber phoneNumber in vcard.PhoneNumbers)
            {
                vCardString += Environment.NewLine;
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    vCardString += "TEL:" + phoneNumber.Number;
                }
                else if(phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    vCardString += "TEL;MAIN-NUMBER:" + phoneNumber.Number;
                }
                else
                {
                    vCardString += "TEL;" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number;
                }
            }
            foreach (EmailAddress email in vcard.EmailAddresses)
            {
                vCardString += Environment.NewLine;
                if (email.Type == EmailType.None)
                {
                    vCardString += "EMAIL:" + email.Email.Address;
                }
                else
                {
                    vCardString += "EMAIL;" + email.Type.ToString().ToUpper() + ":" + email.Email.Address;
                }
            }
            foreach (Address address in vcard.Addresses)
            {
                vCardString += Environment.NewLine;
                if (address.Type == AddressType.None)
                {
                    vCardString += "ADR:" + address.Location;
                }
                else
                {
                    vCardString += "ADR;" + address.Type.ToString().ToUpper() + ":" + address.Location;
                }
            }
            foreach (Photo photo in vcard.Pictures)
            {
                vCardString += Environment.NewLine;
                vCardString += "PHOTO;" + photo.Encoding;
                if (photo.Type == PhotoType.URL)
                {
                    vCardString += ":" + photo.PhotoURL;
                }
                else if (photo.Type == PhotoType.Image)
                {
                    vCardString += ";ENCODING=BASE64:" + photo.ToBase64String();
                }
            }
            foreach (Expertise expertise in vcard.Expertises)
            {
                vCardString += Environment.NewLine;
                vCardString += "EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area;
            }
            foreach (Hobby hobby in vcard.Hobbies)
            {
                vCardString += Environment.NewLine;
                vCardString += "HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity;
            }
            foreach (Interest interest in vcard.Interests)
            {
                vCardString += Environment.NewLine;
                vCardString += "INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity;
            }
            vCardString += Environment.NewLine;
            vCardString += "END:VCARD";
            return vCardString;
        }
    }
}
