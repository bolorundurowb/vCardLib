using System;
using System.IO;
using System.Linq;
using vCardLib.Collections;
using vCardLib.Helpers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Deserializers
{
    public class Deserializer
    {
        public static vCardCollection FromFile(string filePath)
        {
            StreamReader streamReader = Helper.GetStreamReaderFromFile(filePath);
            return FromStreamReader(streamReader);
        }

        public static vCardCollection FromStreamReader(StreamReader streamReader)
        {
            vCardCollection collection = new vCardCollection();
            string contactsString = Helper.GetStringFromStreamReader(streamReader);
            string[] contacts = Helper.GetContactsArrayFromString(contactsString);
            foreach(string contact in contacts)
            {
                string[] contactDetails = Helper.GetContactDetailsArrayFromString(contact);
                if (contactDetails.Length > 0)
                {
                    vCard details = GetVcardFromDetails(contactDetails);
                    collection.Add(details);
                }
            }
            return collection;
        }

        public static vCard GetVcardFromDetails(string[] contactDetails)
        {
			if (contactDetails == null || contactDetails.Length == 0)
			{
				throw new InvalidDataException("the details cannot be null or empty");
			}
            string versionString = contactDetails.FirstOrDefault(s => s.StartsWith("VERSION:"));
            if (versionString == null)
            {
                throw new InvalidOperationException("details do not contain a specification for 'Version'.");
            }
            var version = float.Parse(versionString.Replace("VERSION:", "").Trim());
            vCard vcard = null;
            if (version.Equals(2f) || version.Equals(2.1f))
            {
                vcard = V2Deserializer.Parse(contactDetails);
                vcard.Version = Version.V2;
            }
            else if (version.Equals(3f))
            {
                vcard = V3Deserializer.Parse(contactDetails);
                vcard.Version = Version.V3;
            }
            else if (version.Equals(4.0f))
            {
                vcard = V4Deserializer.Parse(contactDetails);
                vcard.Version = Version.V4;
            }
            return vcard;
        }
    }
}
