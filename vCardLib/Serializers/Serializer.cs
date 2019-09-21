using System;
using System.Text;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Entry point for serializing vCard objects
    /// </summary>
    internal abstract class Serializer
    {
        internal StringBuilder StringBuilder;

        /// <summary>
        /// Writes a vcard object to a file
        /// </summary>
        /// <param name="vcard">The vcard object to be written</param>
        internal void SerializeSharedProperties(vCard vcard)
        {
            StringBuilder = new StringBuilder();
            StringBuilder.Append("BEGIN:VCARD" + Environment.NewLine);
            StringBuilder.Append("REV:" + DateTime.Now.ToString("yyyyMMddTHHmmssZ") + Environment.NewLine);
            StringBuilder.Append(
                $"N:{vcard.FamilyName};{vcard.GivenName};{vcard.MiddleName};{vcard.Prefix};{vcard.Suffix}{Environment.NewLine}"
            );
            StringBuilder.Append("FN:" + vcard.FormattedName + Environment.NewLine);
            if (!string.IsNullOrEmpty(vcard.Organization))
            {
                StringBuilder.Append("ORG:" + vcard.Organization + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Title))
            {
                StringBuilder.Append("TITLE:" + vcard.Title + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Url))
            {
                StringBuilder.Append("URL:" + vcard.Url + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.NickName))
            {
                StringBuilder.Append("NICKNAME:" + vcard.NickName + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Language))
            {
                StringBuilder.Append("LANG:" + vcard.Language + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.BirthPlace))
            {
                StringBuilder.Append("BIRTHPLACE:" + vcard.BirthPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.DeathPlace))
            {
                StringBuilder.Append("DEATHPLACE:" + vcard.DeathPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.TimeZone))
            {
                StringBuilder.Append("TZ:" + vcard.TimeZone + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(vcard.Note))
            {
                StringBuilder.Append("NOTE:" + vcard.Note);
            }

            StringBuilder.Append("KIND:" + vcard.Kind.ToString().ToUpper() + Environment.NewLine);
            StringBuilder.Append("GENDER:" + vcard.Gender.ToString().ToUpper() + Environment.NewLine);

            if (vcard.Geo != null)
            {
                StringBuilder.Append("GEO:" + vcard.Geo.Longitude + ";" + vcard.Geo.Latitude);
            }

            if (vcard.BirthDay != null)
            {
                var birthDay = (DateTime) vcard.BirthDay;
                StringBuilder.Append("BDAY:" + birthDay.Year + birthDay.Month.ToString("00") +
                                     birthDay.Day.ToString("00"));
            }
        }
    }
}
