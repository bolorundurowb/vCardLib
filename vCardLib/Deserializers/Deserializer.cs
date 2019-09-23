using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Utils;

namespace vCardLib.Deserializers
{
    /// <summary>
    /// The entry class for all deserializer tasks
    /// </summary>
    public static class Deserializer
    {
        private static readonly string[] SupportedFields =
        {
            "BEGIN", "VERSION", "N:", "FN:", "ORG", "TITLE", "PHOTO", "TEL", "ADR", "EMAIL", "REV", "TZ", "KIND", "URL",
            "LANG", "NICKNAME", "BIRTHPLACE", "DEATHPLACE", "BDAY", "NOTE", "GENDER", "X-SKYPE-DISPLAYNAME",
            "X-SKYPE-PSTNNUMBER", "GEO", "HOBBY", "EXPERTISE", "INTEREST", "END"
        };

        private static string[] _contactDetails;

        /// <summary>
        /// Retrieves a vcard collection object from a given vcard file
        /// </summary>
        /// <param name="filePath">Path to the vcf or vcard file</param>
        /// <returns>A <see cref="vCardCollection"/></returns>
        public static List<vCard> FromFile(string filePath)
        {
            var streamReader = Helper.GetStreamReaderFromFile(filePath);
            return FromStreamReader(streamReader);
        }

        /// <summary>
        /// Retrieves a vcard
        /// </summary>
        /// <param name="streamReader"><see cref="StreamReader"/> containing a vcard(s)</param>
        /// <returns>A <see cref="vCardCollection"/></returns>
        public static List<vCard> FromStreamReader(StreamReader streamReader)
        {
            var collection = new List<vCard>();
            var contactsString = Helper.GetStringFromStreamReader(streamReader);
            var contacts = Helper.GetContactsArrayFromString(contactsString);
            foreach (var contact in contacts)
            {
                var contactDetails = Helper.GetContactDetailsArrayFromString(contact);
                if (contactDetails.Length <= 0) continue;
                var details = GetVcardFromDetails(contactDetails);
                collection.Add(details);
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

            var versionString = contactDetails.FirstOrDefault(s => s.StartsWith("VERSION:"));
            if (versionString == null)
            {
                throw new InvalidOperationException("details do not contain a specification for 'Version'.");
            }

            var decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var version = float.Parse(versionString
                .Replace("VERSION:", "")
                .Trim()
                .Replace(".", decimalSeparator));
            vCard vcard = null;
            if (version.Equals(2f) || version.Equals(2.1f))
            {
                vcard = Deserialize(contactDetails, vCardVersion.V2);
            }
            else if (version.Equals(3f))
            {
                vcard = Deserialize(contactDetails, vCardVersion.V3);
            }
            else if (version.Equals(4.0f))
            {
                vcard = Deserialize(contactDetails, vCardVersion.V4);
            }

            return vcard;
        }

        /// <summary>
        /// Central point from which all deserializing starts
        /// </summary>
        /// <param name="contactDetails">A string array of the contact details</param>
        /// <param name="version">The version to be deserialized from</param>
        /// <returns>A <see cref="vCard"/> comtaining the contacts details</returns>
        private static vCard Deserialize(string[] contactDetails, vCardVersion version)
        {
            _contactDetails = contactDetails;
            var vcard = new vCard
            {
                Version = version,
                BirthDay = ParseBirthDay(),
                BirthPlace = ParseBirthPlace(),
                DeathPlace = ParseDeathPlace(),
                FamilyName = ParseFamilyName(),
                FormattedName = ParseFormattedName(),
                Geo = ParseGeo(),
                Gender = ParseGender(),
                GivenName = ParseGivenName(),
                Kind = ParseKind(),
                Language = ParseLanguage(),
                MiddleName = ParseMiddleName(),
                NickName = ParseNickname(),
                Note = ParseNote(),
                Organization = ParseOrganization(),
                Prefix = ParsePrefix(),
                Revision = ParseRevision(),
                Suffix = ParseSuffix(),
                TimeZone = ParseTimeZone(),
                Title = ParseTitle(),
                Url = ParseUrl(),
            };

            vcard.CustomFields = vcard.CustomFields ?? new List<KeyValuePair<string, string>>();
            foreach (var contactDetail in _contactDetails)
            {
                if (SupportedFields.Any(x => contactDetail.StartsWith(x)))
                {
                    continue;
                }

                var contactDetailParts = contactDetail.Split(':');
                if (contactDetailParts.Length <= 1)
                {
                    continue;
                }

                var entry = new KeyValuePair<string, string>(contactDetailParts[0],
                    string.Join("", contactDetailParts.Slice(1)));
                vcard.CustomFields.Add(entry);
            }

            switch (version)
            {
                case vCardVersion.V2:
                    return V2Deserializer.Parse(contactDetails, vcard);
                case vCardVersion.V3:
                    return V3Deserializer.Parse(contactDetails, vcard);
                case vCardVersion.V4:
                    return V4Deserializer.Parse(contactDetails, vcard);
                default:
                    throw new ArgumentException($"The version {version} is not supported.");
            }
        }

        /// <summary>
        /// Gets the contact kind from the details array
        /// </summary>
        /// <returns>A <see cref="ContactType"/></returns>
        private static ContactType ParseKind()
        {
            var contactKindString = _contactDetails.FirstOrDefault(s => s.StartsWith("KIND:"));
            if (contactKindString != null)
            {
                contactKindString = contactKindString.Replace("KIND:", "").Trim();
                if (contactKindString == "INDIVIDUAL")
                    return ContactType.Individual;
                if (contactKindString == "GROUP")
                    return ContactType.Group;
                if (contactKindString == "ORG")
                    return ContactType.Organization;
                if (contactKindString == "LOCATION")
                    return ContactType.Location;
                if (contactKindString == "APPLICATION")
                    return ContactType.Application;
                if (contactKindString == "DEVICE")
                    return ContactType.Device;
            }

            return ContactType.Individual;
        }

        /// <summary>
        /// Gets the url from the details array
        /// </summary>
        /// <returns>A string representing the url or an empty string</returns>
        private static string ParseUrl()
        {
            var urlString = _contactDetails.FirstOrDefault(s => s.StartsWith("URL:"));
            if (urlString != null)
                return urlString.Replace("URL:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the title from the details array
        /// </summary>
        /// <returns>A string representing the title or an empty string</returns>
        private static string ParseTitle()
        {
            var titleString = _contactDetails.FirstOrDefault(s => s.StartsWith("TITLE:"));
            if (titleString != null)
            {
                return titleString.Replace("TITLE:", "").Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the organization from the details array
        /// </summary>
        /// <returns>A string representing the organization or an empty string</returns>
        private static string ParseOrganization()
        {
            var orgString = _contactDetails.FirstOrDefault(s => s.StartsWith("ORG:"));
            if (orgString != null)
                return orgString.Replace("ORG:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the language from the details array
        /// </summary>
        /// <returns>A string representing the language or an empty string</returns>
        private static string ParseLanguage()
        {
            var langString = _contactDetails.FirstOrDefault(s => s.StartsWith("LANG:"));
            if (langString != null)
                return langString.Replace("LANG:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the nickname from the details array
        /// </summary>
        /// <returns>A string representing the nickname or an empty string</returns>
        private static string ParseNickname()
        {
            var nicknameString = _contactDetails.FirstOrDefault(s => s.StartsWith("NICKNAME:"));
            if (nicknameString != null)
                return nicknameString.Replace("NICKNAME:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the birth place from the details array
        /// </summary>
        /// <returns>A string representing the birth place or an empty string</returns>
        private static string ParseBirthPlace()
        {
            var birthplaceString = _contactDetails.FirstOrDefault(s => s.StartsWith("BIRTHPLACE:"));
            if (birthplaceString != null)
                return birthplaceString.Replace("BIRTHPLACE:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the death place from the details array
        /// </summary>
        /// <returns>A string representing the death place or an empty string</returns>
        private static string ParseDeathPlace()
        {
            var deathplaceString = _contactDetails.FirstOrDefault(s => s.StartsWith("DEATHPLACE:"));
            if (deathplaceString != null)
            {
                return deathplaceString.Replace("DEATHPLACE:", "").Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the birthday from the details array
        /// </summary>
        /// <returns>A date time object representing the birthday or null</returns>
        private static DateTime? ParseBirthDay()
        {
            var bdayString = _contactDetails.FirstOrDefault(s => s.StartsWith("BDAY:"));
            if (bdayString != null)
            {
                bdayString = bdayString
                    .Replace("BDAY:", "")
                    .Replace("-", "")
                    .Trim();
                DateTime birthday;
                const string format = "yyyyMMdd";
                const DateTimeStyles dateTimeStyle = DateTimeStyles.None;
                IFormatProvider provider = new CultureInfo("en-US");
                if (DateTime.TryParseExact(bdayString, format, provider, dateTimeStyle, out birthday))
                {
                    return birthday;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the family name from the details array
        /// </summary>
        /// <returns>A string representing the family name or an empty string</returns>
        private static string ParseFormattedName()
        {
            var fnString = _contactDetails.FirstOrDefault(s => s.StartsWith("FN:"));
            var formattedName = fnString?.Replace("FN:", "");
            return formattedName;
        }

        /// <summary>
        /// Gets the family name from the details array
        /// </summary>
        /// <returns>A string representing the family name or an empty string</returns>
        private static string ParseFamilyName()
        {
            var nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] {";"}, StringSplitOptions.None);
            if (names?.Length > 0)
                return names[0];
            return string.Empty;
        }

        /// <summary>
        /// Gets the given name from the details array
        /// </summary>
        /// <returns>A string representing the given name or an empty string</returns>
        private static string ParseGivenName()
        {
            var nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] {";"}, StringSplitOptions.None);
            if (names?.Length > 1)
                return names[1];
            return string.Empty;
        }

        /// <summary>
        /// Gets the middle name from the details array
        /// </summary>
        /// <returns>A string representing the middle name or an empty string</returns>
        private static string ParseMiddleName()
        {
            var nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] {";"}, StringSplitOptions.None);
            if (names?.Length > 2)
                return names[2];
            return string.Empty;
        }

        /// <summary>
        /// Gets the prefix from the details array
        /// </summary>
        /// <returns>A string representing the prefix or an empty string</returns>
        private static string ParsePrefix()
        {
            var nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] {";"}, StringSplitOptions.None);
            if (names?.Length > 3)
                return names[3];
            return string.Empty;
        }

        /// <summary>
        /// Gets the suffix from the details array
        /// </summary>
        /// <returns>A string representing the suffix or an empty string</returns>
        private static string ParseSuffix()
        {
            var nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] {";"}, StringSplitOptions.None);
            if (names?.Length > 4)
                return names[4];
            return string.Empty;
        }

        /// <summary>
        /// Gets the time zone from the details array
        /// </summary>
        /// <returns>A string representing the time zone or an empty string</returns>
        private static string ParseTimeZone()
        {
            var tzString = _contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                return tzString.Replace("TZ:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the time zone from the details array
        /// </summary>
        /// <returns>A string representing the time zone or an empty string</returns>
        private static string ParseNote()
        {
            var noteString = _contactDetails.FirstOrDefault(s => s.StartsWith("NOTE"));
            if (noteString != null)
                return noteString.Replace("NOTE:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the gender from the details array
        /// </summary>
        /// <returns>A <see cref="GenderType"/> representing the gender</returns>
        private static GenderType ParseGender()
        {
            var genderString = _contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
            if (genderString != null)
            {
                genderString = genderString.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male" || genderString.ToLower() == "m")
                    return GenderType.Male;
                if (genderString.ToLower() == "female" || genderString.ToLower() == "f")
                    return GenderType.Female;
                return GenderType.Other;
            }

            return GenderType.None;
        }

        /// <summary>
        /// Gets the Skype display name
        /// </summary>
        /// <returns>A string with the skype display name or an empty string</returns>
        private static string ParseXSkypeDisplayName()
        {
            var xSkypeDisplayNumberString = _contactDetails.FirstOrDefault(x => x.StartsWith("X-SKYPE-DISPLAYNAME"));
            if (xSkypeDisplayNumberString != null)
            {
                return xSkypeDisplayNumberString.Replace("X-SKYPE-DISPLAYNAME:", "");
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the Skype PSTN number
        /// </summary>
        /// <returns>A string with the skype PSTN number or an empty string</returns>
        private static string ParseXSkypePstnNumber()
        {
            var xSkypePstnString = _contactDetails.FirstOrDefault(x => x.StartsWith("X-SKYPE-PSTNNUMBER"));
            if (xSkypePstnString != null)
            {
                return xSkypePstnString.Replace("X-SKYPE-PSTNNUMBER:", "");
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the revision of this vcard
        /// </summary>
        /// <returns>A date time object or null</returns>
        private static DateTime? ParseRevision()
        {
            var revisionString = _contactDetails.FirstOrDefault(x => x.StartsWith("REV"));
            if (revisionString != null)
            {
                revisionString = revisionString
                    .Replace("REV:", "")
                    .Replace("-", "")
                    .Trim();
                DateTime revision;
                var format = "yyyyMMddTHHmmssZ";
                var dateTimeStyle = DateTimeStyles.None;
                IFormatProvider provider = new CultureInfo("en-US");
                if (DateTime.TryParseExact(revisionString, format, provider, dateTimeStyle, out revision))
                {
                    return revision;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the location from the details array
        /// </summary>
        /// <returns>A <see cref="Geo"/> stating the longitude and latitude or null</returns>
        private static Geo ParseGeo()
        {
            var geoString = _contactDetails.FirstOrDefault(x => x.StartsWith("GEO"));
            if (geoString != null)
            {
                geoString = geoString.Replace("GEO:", "");
                var geoParts = geoString.Split(';');
                if (geoParts.Length == 2)
                {
                    var longSuccess = double.TryParse(geoParts[0], out _);
                    var latSuccess = double.TryParse(geoParts[1], out var latitude);
                    if (longSuccess && latSuccess)
                    {
                        var geo = new Geo {Latitude = latitude, Longitude = latitude};
                        return geo;
                    }
                }
            }

            return null;
        }
    }
}
