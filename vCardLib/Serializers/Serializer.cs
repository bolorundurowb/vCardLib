using System;
using System.IO;
using vCardLib.Collections;
using vCardLib.Helpers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Entry point for serializing vCard objects
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// Writes a vcard object to a file
        /// </summary>
        /// <param name="vcard">The vcard object to be written</param>
        /// <param name="filePath">The path the vcard should be saved to</param>
        /// <param name="version">The version to be serialized into</param>
        /// <param name="options">State whether the card should be overwritten if it exists</param>
        /// <returns>A value stating if the serialization was successful or not</returns>
        /// <exception cref="InvalidOperationException">Thrown when the file path exists and the overwrite option is not invoked</exception>
        /// <exception cref="ArgumentNullException">Thrown when the vcard supplied is null</exception>
        public static bool Serialize(vCard vcard, string filePath, Version version, WriteOptions options = WriteOptions.ThrowError)
        {
            if (options == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException(
                        "A file with the given filePath exists."
                        + " If you want to overwrite the file,"
                        + " then call this method and pass the "
                        + "optional overwrite option"
                    );
                }
            }
            if (vcard == null)
            {
                throw new ArgumentNullException("The vcard cannot be null.");
            }
            if (version == Version.V2)
            {
                try
                {
                    string vcfString = V2Serializer.Serialize(vcard);
                    File.WriteAllText(filePath, vcfString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else if (version == Version.V3)
            {
                try
                {
                    string vcfString = V3Serializer.Serialize(vcard);
                    File.WriteAllText(filePath, vcfString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else if (version == Version.V4)
            {
                V4Serializer.Serialize(vcard);
            }
            return true;
        }

        /// <summary>
        /// Writes a vcard collection object to a file
        /// </summary>
        /// <param name="vcardCollection">The vcard collection object to be written</param>
        /// <param name="filePath">The path the collection should be saved to</param>
        /// <param name="version">The version to be serialized into</param>
        /// <param name="options">tate whether the card should be overwritten if it exists</param>
        /// <returns>A value stating if the serialization was successful or not</returns>
        /// <exception cref="InvalidOperationException">Thrown when the file path exists and the overwrite option is not invoked</exception>
        /// <exception cref="ArgumentNullException">Thrown when the vcard supplied is null</exception>
        public static bool Serialize(vCardCollection vcardCollection, string filePath, Version version,
            WriteOptions options = WriteOptions.ThrowError)
        {
            if (options == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException(
                        "A file with the given filePath exists."
                        + " If you want to overwrite the file,"
                        + " then call this method and pass the "
                        + "optional overwrite option"
                    );
                }
            }
            if (vcardCollection == null)
            {
                throw new ArgumentNullException("The vcard collection cannot be null.");
            }
            var vcardCollectionString = "";
            if (version == Version.V2)
            {
                foreach(vCard vcard in vcardCollection)
                {
                    vcardCollectionString += Serialize(vcard, Version.V2);
                }
            }
            else if (version == Version.V3)
            {
                foreach (vCard vcard in vcardCollection)
                {
                    vcardCollectionString += Serialize(vcard, Version.V3);
                }
            }
            else
            {
                foreach (vCard vcard in vcardCollection)
                {
                    vcardCollectionString += Serialize(vcard, Version.V4);
                }
            }
            try
            {
                File.WriteAllText(filePath, vcardCollectionString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private static string Serialize(vCard vcard, Version version)
        {
            string vCardString = "";
            vCardString += "BEGIN:VCARD" + Environment.NewLine;

            vCardString += "REV:" + DateTime.Now.ToString("yyyyMMddTHHmmssZ") + Environment.NewLine;
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

            if (version == Version.V2)
            {
                vCardString += "VERSION:2.1" + Environment.NewLine;
                vCardString += V2Serializer.Serialize(vcard);
            }
            else if (version == Version.V3)
            {
                vCardString += "VERSION:3.0" + Environment.NewLine;
                vCardString += V3Serializer.Serialize(vcard);
            }
            else
            {
                vCardString += "VERSION:4.0" + Environment.NewLine;
                vCardString += V4Serializer.Serialize(vcard);
            }
            vCardString += Environment.NewLine;
            vCardString += "END:VCARD";
            return vCardString;
        }
    }
}
