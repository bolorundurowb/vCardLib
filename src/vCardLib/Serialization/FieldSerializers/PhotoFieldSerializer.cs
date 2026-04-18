using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class PhotoFieldSerializer : IV2FieldSerializer<Photo>, IV3FieldSerializer<Photo>, IV4FieldSerializer<Photo>
{
    public string FieldKey => "PHOTO";

    string? IV2FieldSerializer<Photo>.Write(Photo data)
    {
        var builder = new StringBuilder(FieldKey);

        if (!string.IsNullOrWhiteSpace(data.Type))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.Type);
        }

        if (!string.IsNullOrWhiteSpace(data.Encoding))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.EncodingKey, data.Encoding);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Data);

        return builder.ToString();
    }

    string? IV3FieldSerializer<Photo>.Write(Photo data)
    {
        var builder = new StringBuilder(FieldKey);

        if (!string.IsNullOrWhiteSpace(data.Type))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.Type);
        }

        if (!string.IsNullOrWhiteSpace(data.Value))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.ValueKey, data.Value);
        }

        if (!string.IsNullOrWhiteSpace(data.Encoding))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.EncodingKey, data.Encoding == "BASE64" ? "b" : data.Encoding);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }

    string? IV4FieldSerializer<Photo>.Write(Photo data)
    {
        var builder = new StringBuilder(FieldKey);

        if (!string.IsNullOrWhiteSpace(data.Type))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.Type);
        }

        if (!string.IsNullOrWhiteSpace(data.Value))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.ValueKey, data.Value);
        }

        if (!string.IsNullOrWhiteSpace(data.Encoding))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.EncodingKey, data.Encoding);
        }

        if (!string.IsNullOrWhiteSpace(data.MimeType))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.AppendFormat("{0}={1}", FieldKeyConstants.MediaTypeKey, data.MimeType);
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Value);

        return builder.ToString();
    }
}
