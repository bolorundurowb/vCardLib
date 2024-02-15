using System.Text;
using vCardLib.Constants;
using vCardLib.Extensions;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class KeyFieldSerializer : IV2FieldSerializer<Key>, IV3FieldSerializer<Key>, IV4FieldSerializer<Key>
{
    public string FieldKey => "KEY";

    string? IV2FieldSerializer<Key>.Write(Key data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.Type);
        }

        if (data.Encoding != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.EncodingKey, data.Encoding);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }

    string? IV3FieldSerializer<Key>.Write(Key data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, data.Type);
        }

        if (data.Encoding != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.EncodingKey,
                data.Encoding.EqualsIgnoreCase("BASE64") ? "b" : data.Encoding);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }

    string? IV4FieldSerializer<Key>.Write(Key data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, data.Type);
        }

        if (data.MimeType != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.MediaTypeKey, data.MimeType);
        }

        if (data.Encoding != null && data.Encoding.EqualsIgnoreCase("BASE64"))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("base64,{0}", data.Value);
        }
        else
        {
            builder.Append(FieldKeyConstants.SectionDelimiter);
            builder.Append(data.Value);
        }

        return builder.ToString();
    }
}
