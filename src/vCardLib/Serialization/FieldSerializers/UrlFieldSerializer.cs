using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class UrlFieldSerializer : IV2FieldSerializer<Url>, IV3FieldSerializer<Url>,
    IV4FieldSerializer<Url>
{
    public string FieldKey => "URL";

    string IV2FieldSerializer<Url>.Write(Url data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type.HasValue)
        {
            var urlTypes = EnumExtensions.Values(data.Type.Value);

            if (urlTypes.Any())
            {
                foreach (var urlType in urlTypes)
                {
                    builder.Append(FieldKeyConstants.MetadataDelimiter);
                    builder.Append(urlType.ToString().ToUpperInvariant());
                }
            }
        }

        WriteOtherAttributes(data, builder);

        return builder.ToString();
    }

    public string Write(Url data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type.HasValue)
        {
            var urlTypes = EnumExtensions.Values(data.Type.Value);

            if (urlTypes.Any())
            {
                foreach (var urlType in urlTypes)
                {
                    builder.Append(FieldKeyConstants.MetadataDelimiter);
                    builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, urlType.ToString().ToLowerInvariant());
                }
            }
        }

        WriteOtherAttributes(data, builder);

        return builder.ToString();
    }

    private static void WriteOtherAttributes(Url data, StringBuilder builder)
    {
        if (data.Preference.HasValue)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.PreferenceKey, data.Preference);
        }

        if (!string.IsNullOrWhiteSpace(data.Label))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            var hasWhitespace = data.Label!.Any(char.IsWhiteSpace);
            builder.AppendFormat(hasWhitespace ? "{0}={1}" : "\"{0}={1}\"", FieldKeyConstants.LabelKey, data.Label);
        }

        if (!string.IsNullOrWhiteSpace(data.MimeType))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.MediaTypeAltKey, data.MimeType);
        }

        if (!string.IsNullOrWhiteSpace(data.Language))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.LanguageSetKey, data.Language);
        }

        if (!string.IsNullOrWhiteSpace(data.Charset))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.CharacterSetKey, data.Charset);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);
    }
}