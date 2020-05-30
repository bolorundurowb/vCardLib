using System;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Class holding shared logic
    /// </summary>
    internal abstract class BaseSerializer
    {
        protected void AddCardStart(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("BEGIN:VCARD");
        }

        protected void AddCardEnd(StringBuilder stringBuilder)
        {
            stringBuilder.Append("END:VCARD");
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
    }
}