using System;
using System.IO;
using System.Linq;
using vCardLib.Collections;
using vCardLib.Helpers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Deserializers
{
    /// <summary>
    /// The entry class for all deserializer tasks
    /// </summary>
    public class Deserializer
    {
        /// <summary>
        /// Retrieves a vcard collection object from a given vcard file
        /// </summary>
        /// <param name="filePath">Path to the vcf or vcard file</param>
        /// <returns>A <see cref="vCardCollection"/></returns>
        public static vCardCollection FromFile(string filePath)
        {
            StreamReader streamReader = Helper.GetStreamReaderFromFile(filePath);
            return FromStreamReader(streamReader);
        }

        /// <summary>
        /// Retrieves a vcard
        /// </summary>
        /// <param name="streamReader"><see cref="StreamReader"/> containing a vcard(s)</param>
        /// <returns>A <see cref="vCardCollection"/></returns>
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

        /// <summary>
        /// Creates a vcard object from an array of vcard properties
        /// </summary>
        /// <param name="contactDetails">A string array of vcard properties</param>
        /// <returns>A <see cref="vCard"/> object</returns>
        /// <exception cref="InvalidDataException">When the array is null or empty</exception>
        /// <exception cref="InvalidOperationException">When  no version is stated</exception>
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
