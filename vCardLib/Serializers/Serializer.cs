using System;
using System.Text;
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

        private void AddCardStart()
        {
            _stringBuilder.Append("BEGIN:VCARD").AppendLine();
        }

        private void AddCardEnd()
        {
            _stringBuilder.AppendLine().Append("END:VCARD");
        }

        private void AddSharedFields()
        {
            _stringBuilder.Append("REV:" + DateTime.Now.ToString("yyyyMMddTHHmmssZ"))
                .Append(Environment.NewLine);

            _stringBuilder.Append(
                $"N:{_vCard.FamilyName};{_vCard.GivenName};{_vCard.MiddleName};{_vCard.Prefix};{_vCard.Suffix}{Environment.NewLine}"
            );

            _stringBuilder.Append("FN:" + _vCard.FormattedName)
                .Append(Environment.NewLine);

            if (!string.IsNullOrEmpty(_vCard.Organization))
            {
                _stringBuilder.Append("ORG:" + _vCard.Organization)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Title))
            {
                _stringBuilder.Append("TITLE:" + _vCard.Title)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Url))
            {
                _stringBuilder.Append("URL:" + _vCard.Url)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.NickName))
            {
                _stringBuilder.Append("NICKNAME:" + _vCard.NickName)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Language))
            {
                _stringBuilder.Append("LANG:" + _vCard.Language)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.BirthPlace))
            {
                _stringBuilder.Append("BIRTHPLACE:" + _vCard.BirthPlace)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.DeathPlace))
            {
                _stringBuilder.Append("DEATHPLACE:" + _vCard.DeathPlace)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.TimeZone))
            {
                _stringBuilder.Append("TZ:" + _vCard.TimeZone)
                    .Append(Environment.NewLine);
            }

            if (!string.IsNullOrEmpty(_vCard.Note))
            {
                _stringBuilder.Append("NOTE:" + _vCard.Note)
                    .Append(Environment.NewLine);
            }

            _stringBuilder.Append("KIND:" + _vCard.Kind.ToString().ToUpper())
                .Append(Environment.NewLine);
            
            _stringBuilder.Append("GENDER:" + _vCard.Gender.ToString().ToUpper())
                .Append(Environment.NewLine);

            if (_vCard.Geo != null)
            {
                _stringBuilder.Append("GEO:" + _vCard.Geo.Longitude + ";" + _vCard.Geo.Latitude);
            }

            if (_vCard.BirthDay.HasValue)
            {
                _stringBuilder
                    .Append("BDAY:")
                    .Append(_vCard.BirthDay.Value.ToString("yyyyMMdd"))
                    .Append(Environment.NewLine);
            }
        }
    }
}