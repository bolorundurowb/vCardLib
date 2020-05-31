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
    public abstract class Deserializer
    {
        private readonly string[] _supportedFields =
        {
            "BEGIN", "VERSION", "N:", "FN:", "ORG", "TITLE", "PHOTO", "TEL", "ADR", "EMAIL", "REV", "TZ", "KIND", "URL",
            "LANG", "NICKNAME", "BIRTHPLACE", "DEATHPLACE", "BDAY", "NOTE", "GENDER", "GEO", "HOBBY", "EXPERTISE",
            "INTEREST", "END"
        };

        /// <summary>
        /// Retrieves a vcard collection object from a given vcard file
        /// </summary>
        /// <param name="filePath">Path to the vcf or vcard file</param>
        /// <returns>A <see cref="List<vCard>"/></returns>
        public static List<vCard> FromFile(string filePath)
        {
            var contacts = Helpers.GetContactsFromFile(filePath);
            return CreateCardsFromContacts(contacts);
        }

        /// <summary>
        /// Retrieves a vcard
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> containing a vcard(s)</param>
        /// <returns>A <see cref="List<vCard>"/></returns>
        public static List<vCard> FromStream(Stream stream)
        {
            var contacts = Helpers.GetContactsFromStream(stream);
            return CreateCardsFromContacts(contacts);
        }

        /// <summary>
        /// Retrieves a vcard
        /// </summary>
        /// <param name="contents">A string containing a vcard(s)</param>
        /// <returns>A <see cref="List<vCard>"/></returns>
        public static List<vCard> FromString(string contents)
        {
            var contacts = Helpers.GetContactsFromString(contents);
            return CreateCardsFromContacts(contacts);
        }

        private static List<vCard> CreateCardsFromContacts(IEnumerable<string[]> contacts)
        {
            return contacts
                .Select(GetCardFromContact)
                .ToList();
        }

        private static vCard GetCardFromContact(string[] contact)
        {
            var versionString = contact
                .FirstOrDefault(s => s.StartsWith("VERSION:"));

            if (versionString == null)
            {
                throw new InvalidOperationException("Details do not contain a specification for 'Version'.");
            }

            var decimalSeparator = CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
            var version = float.Parse(versionString
                .Replace("VERSION:", "")
                .Trim()
                .Replace(".", decimalSeparator));

            Deserializer deserializer;
            if (version >= 2f && version < 3f)
            {
                deserializer = new v2Deserializer();
            }
            else if (version >= 3f && version < 4f)
            {
                deserializer = new v3Deserializer();
            }
            else if (version.Equals(4.0f))
            {
                deserializer = new v4Deserializer();
            }
            else
            {
                throw new NotSupportedException();
            }

            return deserializer.Deserialize(contact);
        }

        protected vCard Deserialize(string[] contactDetails)
        {
            var card = new vCard
            {
                Version = ParseVersion(),
                BirthDay = ParseBirthDay(contactDetails),
                BirthPlace = ParseBirthPlace(contactDetails),
                DeathPlace = ParseDeathPlace(contactDetails),
                FamilyName = ParseFamilyName(contactDetails),
                FormattedName = ParseFormattedName(contactDetails),
                Geo = ParseGeo(contactDetails),
                Gender = ParseGender(contactDetails),
                GivenName = ParseGivenName(contactDetails),
                Kind = ParseKind(contactDetails),
                Language = ParseLanguage(contactDetails),
                MiddleName = ParseMiddleName(contactDetails),
                NickName = ParseNickname(contactDetails),
                Note = ParseNote(contactDetails),
                Organization = ParseOrganization(contactDetails),
                Prefix = ParsePrefix(contactDetails),
                Revision = ParseRevision(contactDetails),
                Suffix = ParseSuffix(contactDetails),
                TimeZone = ParseTimeZone(contactDetails),
                Title = ParseTitle(contactDetails),
                Url = ParseUrl(contactDetails),
                Addresses = ParseAddresses(contactDetails),
                EmailAddresses = ParseEmailAddresses(contactDetails),
                PhoneNumbers = ParsePhoneNumbers(contactDetails),
                Pictures = ParsePhotos(contactDetails),
                Hobbies = ParseHobbies(contactDetails),
                Expertises = ParseExpertises(contactDetails),
                Interests = ParseInterests(contactDetails)
            };

            card.CustomFields = card.CustomFields ?? new List<KeyValuePair<string, string>>();
            foreach (var contactDetail in contactDetails)
            {
                if (_supportedFields.Any(x => contactDetail.StartsWith(x)))
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
                card.CustomFields.Add(entry);
            }

            return card;
        }

        protected ContactType ParseKind(string[] contactDetails)
        {
            var contactKindString = contactDetails.FirstOrDefault(s => s.StartsWith("KIND:"));
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
        protected string ParseUrl(string[] contactDetails)
        {
            var urlString = contactDetails.FirstOrDefault(s => s.StartsWith("URL:"));
            if (urlString != null)
                return urlString.Replace("URL:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the title from the details array
        /// </summary>
        /// <returns>A string representing the title or an empty string</returns>
        protected string ParseTitle(string[] contactDetails)
        {
            var titleString = contactDetails.FirstOrDefault(s => s.StartsWith("TITLE:"));
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
        protected string ParseOrganization(string[] contactDetails)
        {
            var orgString = contactDetails.FirstOrDefault(s => s.StartsWith("ORG:"));
            if (orgString != null)
                return orgString.Replace("ORG:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the language from the details array
        /// </summary>
        /// <returns>A string representing the language or an empty string</returns>
        protected string ParseLanguage(string[] contactDetails)
        {
            var langString = contactDetails.FirstOrDefault(s => s.StartsWith("LANG:"));
            if (langString != null)
                return langString.Replace("LANG:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the nickname from the details array
        /// </summary>
        /// <returns>A string representing the nickname or an empty string</returns>
        protected string ParseNickname(string[] contactDetails)
        {
            var nicknameString = contactDetails.FirstOrDefault(s => s.StartsWith("NICKNAME:"));
            if (nicknameString != null)
                return nicknameString.Replace("NICKNAME:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the birth place from the details array
        /// </summary>
        /// <returns>A string representing the birth place or an empty string</returns>
        protected string ParseBirthPlace(string[] contactDetails)
        {
            var birthplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("BIRTHPLACE:"));
            if (birthplaceString != null)
                return birthplaceString.Replace("BIRTHPLACE:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the death place from the details array
        /// </summary>
        /// <returns>A string representing the death place or an empty string</returns>
        protected string ParseDeathPlace(string[] contactDetails)
        {
            var deathplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("DEATHPLACE:"));
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
        protected DateTime? ParseBirthDay(string[] contactDetails)
        {
            var bdayString = contactDetails.FirstOrDefault(s => s.StartsWith("BDAY:"));
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
        protected string ParseFormattedName(string[] contactDetails)
        {
            var fnString = contactDetails.FirstOrDefault(s => s.StartsWith("FN:"));
            var formattedName = fnString?.Replace("FN:", "");
            return formattedName;
        }

        /// <summary>
        /// Gets the family name from the details array
        /// </summary>
        /// <returns>A string representing the family name or an empty string</returns>
        protected string ParseFamilyName(string[] contactDetails)
        {
            var nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[]
            {
                ";"
            }, StringSplitOptions.None);
            if (names?.Length > 0)
                return names[0];
            return string.Empty;
        }

        /// <summary>
        /// Gets the given name from the details array
        /// </summary>
        /// <returns>A string representing the given name or an empty string</returns>
        protected string ParseGivenName(string[] contactDetails)
        {
            var nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[]
            {
                ";"
            }, StringSplitOptions.None);
            if (names?.Length > 1)
                return names[1];
            return string.Empty;
        }

        /// <summary>
        /// Gets the middle name from the details array
        /// </summary>
        /// <returns>A string representing the middle name or an empty string</returns>
        protected string ParseMiddleName(string[] contactDetails)
        {
            var nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[]
            {
                ";"
            }, StringSplitOptions.None);
            if (names?.Length > 2)
                return names[2];
            return string.Empty;
        }

        /// <summary>
        /// Gets the prefix from the details array
        /// </summary>
        /// <returns>A string representing the prefix or an empty string</returns>
        protected string ParsePrefix(string[] contactDetails)
        {
            var nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[]
            {
                ";"
            }, StringSplitOptions.None);
            if (names?.Length > 3)
                return names[3];
            return string.Empty;
        }

        /// <summary>
        /// Gets the suffix from the details array
        /// </summary>
        /// <returns>A string representing the suffix or an empty string</returns>
        protected string ParseSuffix(string[] contactDetails)
        {
            var nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[]
            {
                ";"
            }, StringSplitOptions.None);
            if (names?.Length > 4)
                return names[4];
            return string.Empty;
        }

        /// <summary>
        /// Gets the time zone from the details array
        /// </summary>
        /// <returns>A string representing the time zone or an empty string</returns>
        protected string ParseTimeZone(string[] contactDetails)
        {
            var tzString = contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                return tzString.Replace("TZ:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the time zone from the details array
        /// </summary>
        /// <returns>A string representing the time zone or an empty string</returns>
        protected string ParseNote(string[] contactDetails)
        {
            var noteString = contactDetails.FirstOrDefault(s => s.StartsWith("NOTE"));
            if (noteString != null)
                return noteString.Replace("NOTE:", "").Trim();
            return string.Empty;
        }

        /// <summary>
        /// Gets the gender from the details array
        /// </summary>
        /// <returns>A <see cref="GenderType"/> representing the gender</returns>
        protected GenderType ParseGender(string[] contactDetails)
        {
            var genderString = contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
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
        /// Gets the revision of this vcard
        /// </summary>
        /// <returns>A date time object or null</returns>
        protected DateTime? ParseRevision(string[] contactDetails)
        {
            var revisionString = contactDetails.FirstOrDefault(x => x.StartsWith("REV"));
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
        protected Geo ParseGeo(string[] contactDetails)
        {
            var geoString = contactDetails.FirstOrDefault(x => x.StartsWith("GEO"));
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
                        var geo = new Geo
                        {
                            Latitude = latitude,
                            Longitude = latitude
                        };
                        return geo;
                    }
                }
            }

            return null;
        }

        protected abstract vCardVersion ParseVersion();

        protected abstract List<Address> ParseAddresses(string[] contactDetails);

        protected abstract List<PhoneNumber> ParsePhoneNumbers(string[] contactDetails);

        protected abstract List<EmailAddress> ParseEmailAddresses(string[] contactDetails);

        protected abstract List<Hobby> ParseHobbies(string[] contactDetails);

        protected abstract List<Expertise> ParseExpertises(string[] contactDetails);

        protected abstract List<Interest> ParseInterests(string[] contactDetails);

        protected abstract List<Photo> ParsePhotos(string[] contactDetails);
    }
}