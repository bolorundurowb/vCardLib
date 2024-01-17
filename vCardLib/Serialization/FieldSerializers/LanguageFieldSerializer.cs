using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class LanguageFieldSerializer : IV2FieldSerializer<Language>, IV3FieldSerializer<Language>,
    IV4FieldSerializer<Language>
{
    public string FieldKey => "LANG";

    public string? Write(Language data) => null;

    string? IV4FieldSerializer<Language>.Write(Language data)
    {
        var builder = new StringBuilder(FieldKey);

        if (!string.IsNullOrWhiteSpace(data.Type))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, data.Type);
        }

        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Locale);

        return builder.ToString();
    }
}
