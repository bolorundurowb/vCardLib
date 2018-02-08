using System;
using System.IO;
using System.Text;
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
        /// <param name="encoding">The encoding to save the file with</param>
        /// <returns>A value stating if the serialization was successful or not</returns>
        /// <exception cref="InvalidOperationException">Thrown when the file path exists and the overwrite option is not invoked</exception>
        /// <exception cref="ArgumentNullException">Thrown when the vcard supplied is null</exception>
        public static bool Serialize(vCard vcard, string filePath, Version version,
            WriteOptions options = WriteOptions.ThrowError, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

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
                    var vcfString = Serialize(vcard, Version.V2);
                    File.WriteAllText(filePath, vcfString, encoding);
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
                    var vcfString = Serialize(vcard, Version.V3);
                    File.WriteAllText(filePath, vcfString, encoding);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else if (version == Version.V4)
            {
                //TODO: once support has been implemented, enable writing capabilities
                /*string vcfString = */
                Serialize(vcard, Version.V4);
                //File.WriteAllText(filePath, vcfString)
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
        /// <param name="encoding">The encoding to save the file with</param>
        /// <returns>A value stating if the serialization was successful or not</returns>
        /// <exception cref="InvalidOperationException">Thrown when the file path exists and the overwrite option is not invoked</exception>
        /// <exception cref="ArgumentNullException">Thrown when the vcard supplied is null</exception>
        public static bool Serialize(vCardCollection vcardCollection, string filePath, Version version,
            WriteOptions options = WriteOptions.ThrowError, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Unicode;
            }

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

            var vcardCollectionString = new StringBuilder();
            switch (version)
            {
                case Version.V2:
                    foreach (vCard vcard in vcardCollection)
                    {
                        vcardCollectionString.Append(Serialize(vcard, Version.V2));
                        vcardCollectionString.Append(Environment.NewLine);
                    }

                    break;
                case Version.V3:
                    foreach (vCard vcard in vcardCollection)
                    {
                        vcardCollectionString.Append(Serialize(vcard, Version.V3));
                        vcardCollectionString.Append(Environment.NewLine);
                    }

                    break;
                default:
                    foreach (vCard vcard in vcardCollection)
                    {
                        vcardCollectionString.Append(Serialize(vcard, Version.V4));
                        vcardCollectionString.Append(Environment.NewLine);
                    }

                    break;
            }

            try
            {
                File.WriteAllText(filePath, vcardCollectionString.ToString(), encoding);
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
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("BEGIN:VCARD" + Environment.NewLine);
            stringBuilder.Append("REV:" + DateTime.Now.ToString("yyyyMMddTHHmmssZ") + Environment.NewLine);
            stringBuilder.Append("N:" + vcard.FamilyName + ";" + vcard.GivenName + ";" + vcard.MiddleName + ";" +
                                 vcard.Prefix + ";" + vcard.Suffix + Environment.NewLine);
            stringBuilder.Append("FN:" + vcard.FormattedName + Environment.NewLine);
            if (!string.IsNullOrEmpty(vcard.Organization))
            {
                stringBuilder.Append("ORG:" + vcard.Organization + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Title))
            {
                stringBuilder.Append("TITLE:" + vcard.Title + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Url))
            {
                stringBuilder.Append("URL:" + vcard.Url + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.NickName))
            {
                stringBuilder.Append("NICKNAME:" + vcard.NickName + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Language))
            {
                stringBuilder.Append("LANG:" + vcard.Language + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.BirthPlace))
            {
                stringBuilder.Append("BIRTHPLACE:" + vcard.BirthPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.DeathPlace))
            {
                stringBuilder.Append("DEATHPLACE:" + vcard.DeathPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.TimeZone))
            {
                stringBuilder.Append("TZ:" + vcard.TimeZone + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Note))
            {
                stringBuilder.Append("NOTE:" + vcard.Note);
            }

            if (!string.IsNullOrEmpty(vcard.XSkypeDisplayName))
            {
                stringBuilder.Append("X-SKYPE-DISPLAYNAME:" + vcard.XSkypeDisplayName + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.XSkypePstnNumber))
            {
                stringBuilder.Append("X-SKYPE-PSTNNUMBER:" + vcard.XSkypePstnNumber + Environment.NewLine);
            }

            stringBuilder.Append("KIND:" + vcard.Kind.ToString().ToUpper() + Environment.NewLine);
            stringBuilder.Append("GENDER:" + vcard.Gender.ToString().ToUpper() + Environment.NewLine);

            if (vcard.Geo != null)
            {
                stringBuilder.Append("GEO:" + vcard.Geo.Longitude + ";" + vcard.Geo.Latitude);
            }

            if (vcard.BirthDay != null)
            {
                var birthDay = (DateTime) vcard.BirthDay;
                stringBuilder.Append("BDAY:" + birthDay.Year + birthDay.Month.ToString("00") +
                                     birthDay.Day.ToString("00"));
            }

            if (version == Version.V2)
            {
                stringBuilder.Append("VERSION:2.1" + Environment.NewLine);
                stringBuilder.Append(V2Serializer.Serialize(vcard));
            }
            else if (version == Version.V3)
            {
                stringBuilder.Append("VERSION:3.0" + Environment.NewLine);
                stringBuilder.Append(V3Serializer.Serialize(vcard));
            }
            else
            {
                stringBuilder.Append("VERSION:4.0" + Environment.NewLine);
                stringBuilder.Append(V4Serializer.Serialize(vcard));
            }

            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append("END:VCARD");
            return stringBuilder.ToString();
        }
    }
}