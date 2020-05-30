using System;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Entry point for serializing vCard objects
    /// </summary>
    internal abstract class Serializer
    {
        protected StringBuilder _stringBuilder;
        protected vCard _vCard;
        protected abstract void AddVersionedFields();

        internal string Serialize(vCard vCard)
        {
            _stringBuilder = new StringBuilder();
            _vCard = vCard;

            // add card start
            AddCardStart();

            // add shared fields
            AddSharedFields();

            // add specialized fields
            AddVersionedFields();

            // add card close
            AddCardEnd();

            return _stringBuilder.ToString();
        }

        private void AddSharedFields()
        {
            if (!string.IsNullOrEmpty(_vCard.Organization))
            {
                _stringBuilder.Append("ORG:" + _vCard.Organization + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Title))
            {
                _stringBuilder.Append("TITLE:" + _vCard.Title + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Url))
            {
                _stringBuilder.Append("URL:" + _vCard.Url + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.NickName))
            {
                _stringBuilder.Append("NICKNAME:" + _vCard.NickName + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Language))
            {
                _stringBuilder.Append("LANG:" + _vCard.Language + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.BirthPlace))
            {
                _stringBuilder.Append("BIRTHPLACE:" + _vCard.BirthPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.DeathPlace))
            {
                _stringBuilder.Append("DEATHPLACE:" + _vCard.DeathPlace + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.TimeZone))
            {
                _stringBuilder.Append("TZ:" + _vCard.TimeZone + Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Note))
            {
                _stringBuilder.Append("NOTE:" + _vCard.Note);
            }

            _stringBuilder.Append("KIND:" + _vCard.Kind.ToString().ToUpper() + Environment.NewLine);
            _stringBuilder.Append("GENDER:" + _vCard.Gender.ToString().ToUpper() + Environment.NewLine);

            if (_vCard.Geo != null)
            {
                
            }

            if (_vCard.BirthDay != null)
            {
                var birthDay = (DateTime) _vCard.BirthDay;
                _
            }
        }

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
            stringBuilder.AppendLine($"FN:{formattedName}");
        }

        protected void AddOrganization(StringBuilder stringBuilder, string organization)
        {
            stringBuilder.AppendLine($"ORG:{organization}");
        }

        protected void AddTitle(StringBuilder stringBuilder, string title)
        {
            stringBuilder.AppendLine($"TITLE:{title}");
        }

        protected void AddUrl(StringBuilder stringBuilder, string url)
        {
            stringBuilder.AppendLine($"URL:{url}");
        }

        protected void AddNickName(StringBuilder stringBuilder, string nickName)
        {
            stringBuilder.AppendLine($"NICKNAME:{nickName}");
        }

        protected void AddLanguage(StringBuilder stringBuilder, string language)
        {
            stringBuilder.AppendLine($"LANG:{language}");
        }

        protected void AddBirthPlace(StringBuilder stringBuilder, string birthPlace)
        {
            stringBuilder.AppendLine($"BIRTHPLACE:{birthPlace}");
        }

        protected void AddDeathPlace(StringBuilder stringBuilder, string deathPlace)
        {
            stringBuilder.AppendLine($"DEATHPLACE:{deathPlace}");
        }

        protected void AddTimeZone(StringBuilder stringBuilder, string timeZone)
        {
            stringBuilder.AppendLine($"TZ:{timeZone}");
        }

        protected void AddNote(StringBuilder stringBuilder, string note)
        {
            stringBuilder.AppendLine($"NOTE:{note}");
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

        protected void AddName(StringBuilder stringBuilder, DateTime? birthDay)
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