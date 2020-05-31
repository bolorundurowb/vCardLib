using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Utils;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Class holding shared logic
    /// </summary>
    public abstract class BaseSerializer
    {
        private void WriteCardToBuilder(StringBuilder stringBuilder, vCard vCard)
        {
            // add fields in order
            AddCardStart(stringBuilder);
            AddVersion(stringBuilder);
            AddRevision(stringBuilder);
            AddName(stringBuilder, vCard.FamilyName, vCard.GivenName, vCard.MiddleName, vCard.Prefix, vCard.Suffix);
            AddFormattedName(stringBuilder, vCard.FormattedName);
            AddOrganization(stringBuilder, vCard.Organization);
            AddTitle(stringBuilder, vCard.Title);
            AddUrl(stringBuilder, vCard.Url);
            AddNickName(stringBuilder, vCard.NickName);
            AddLanguage(stringBuilder, vCard.Language);
            AddBirthPlace(stringBuilder, vCard.BirthPlace);
            AddDeathPlace(stringBuilder, vCard.DeathPlace);
            AddTimeZone(stringBuilder, vCard.TimeZone);
            AddNote(stringBuilder, vCard.Note);
            AddContactKind(stringBuilder, vCard.Kind);
            AddGender(stringBuilder, vCard.Gender);
            AddGeo(stringBuilder, vCard.Geo);
            AddBirthday(stringBuilder, vCard.BirthDay);
            AddPhoneNumbers(stringBuilder, vCard.PhoneNumbers);
            AddEmailAddresses(stringBuilder, vCard.EmailAddresses);
            AddAddresses(stringBuilder, vCard.Addresses);
            AddPhotos(stringBuilder, vCard.Pictures);
            AddExpertises(stringBuilder, vCard.Expertises);
            AddHobbies(stringBuilder, vCard.Hobbies);
            AddInterests(stringBuilder, vCard.Interests);
            AddCardEnd(stringBuilder);
        }

        public string Serialize(vCard vCard)
        {
            if (vCard == null)
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            WriteCardToBuilder(stringBuilder, vCard);
            return stringBuilder.ToString();
        }

        public string Serialize(IEnumerable<vCard> vCardCollection)
        {
            if (vCardCollection == null)
            {
                return string.Empty;
            }

            if (!vCardCollection.Any())
            {
                return string.Empty;
            }

            var stringBuilder = new StringBuilder();
            foreach (var vCard in vCardCollection)
            {
                WriteCardToBuilder(stringBuilder, vCard);
            }

            return stringBuilder.ToString();
        }
        
        protected void AddCardStart(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine(Constants.StartToken);
        }

        protected void AddCardEnd(StringBuilder stringBuilder)
        {
            stringBuilder.Append(Constants.EndToken);
        }

        protected void AddRevision(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine($"REV:{DateTime.Now:yyyyMMddTHHmmssZ}");
        }

        protected void AddName(StringBuilder stringBuilder, string familyName, string givenName, string middleName,
            string prefix, string suffix)
        {
            stringBuilder.AppendLine(
                $"N:{familyName};{givenName};{middleName};{prefix};{suffix}"
            );
        }

        protected void AddFormattedName(StringBuilder stringBuilder, string formattedName)
        {
            if (!string.IsNullOrWhiteSpace(formattedName))
            {
                stringBuilder.AppendLine($"FN:{formattedName}");
            }
        }

        protected void AddOrganization(StringBuilder stringBuilder, string organization)
        {
            if (!string.IsNullOrWhiteSpace(organization))
            {
                stringBuilder.AppendLine($"ORG:{organization}");
            }
        }

        protected void AddTitle(StringBuilder stringBuilder, string title)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                stringBuilder.AppendLine($"TITLE:{title}");
            }
        }

        protected void AddUrl(StringBuilder stringBuilder, string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                stringBuilder.AppendLine($"URL:{url}");
            }
        }

        protected void AddNickName(StringBuilder stringBuilder, string nickName)
        {
            if (!string.IsNullOrWhiteSpace(nickName))
            {
                stringBuilder.AppendLine($"NICKNAME:{nickName}");
            }
        }

        protected void AddLanguage(StringBuilder stringBuilder, string language)
        {
            if (!string.IsNullOrWhiteSpace(language))
            {
                stringBuilder.AppendLine($"LANG:{language}");
            }
        }

        protected void AddBirthPlace(StringBuilder stringBuilder, string birthPlace)
        {
            if (!string.IsNullOrWhiteSpace(birthPlace))
            {
                stringBuilder.AppendLine($"BIRTHPLACE:{birthPlace}");
            }
        }

        protected void AddDeathPlace(StringBuilder stringBuilder, string deathPlace)
        {
            if (!string.IsNullOrWhiteSpace(deathPlace))
            {
                stringBuilder.AppendLine($"DEATHPLACE:{deathPlace}");
            }
        }

        protected void AddTimeZone(StringBuilder stringBuilder, string timeZone)
        {
            if (!string.IsNullOrWhiteSpace(timeZone))
            {
                stringBuilder.AppendLine($"TZ:{timeZone}");
            }
        }

        protected void AddNote(StringBuilder stringBuilder, string note)
        {
            if (!string.IsNullOrWhiteSpace(note))
            {
                stringBuilder.AppendLine($"NOTE:{note}");
            }
        }

        protected void AddContactKind(StringBuilder stringBuilder, ContactType contactType)
        {
            stringBuilder.AppendLine($"KIND:{contactType.ToString()}");
        }

        protected void AddGender(StringBuilder stringBuilder, GenderType genderType)
        {
            stringBuilder.AppendLine($"GENDER:{genderType.ToString()}");
        }

        protected void AddGeo(StringBuilder stringBuilder, Geo geo)
        {
            if (geo != null)
            {
                stringBuilder.Append("GEO:" + geo.Longitude + ";" + geo.Latitude);
            }
        }

        protected void AddBirthday(StringBuilder stringBuilder, DateTime? birthDay)
        {
            if (birthDay.HasValue)
            {
                var bDay = birthDay.Value;
                stringBuilder.Append("BDAY:" + bDay.Year + bDay.Month.ToString("00") +
                                     bDay.Day.ToString("00"));
            }
        }

        protected abstract void AddVersion(StringBuilder stringBuilder);

        protected abstract void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<PhoneNumber> phoneNumbers);

        protected abstract void AddEmailAddresses(StringBuilder stringBuilder,
            IEnumerable<EmailAddress> emailAddresses);

        protected abstract void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses);

        protected abstract void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos);

        protected abstract void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises);

        protected abstract void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies);

        protected abstract void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests);
    }
}
