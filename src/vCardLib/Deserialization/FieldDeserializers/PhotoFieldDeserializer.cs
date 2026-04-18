using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class PhotoFieldDeserializer : IV2FieldDeserializer<Photo>,
    IV3FieldDeserializer<Photo>, IV4FieldDeserializer<Photo>
{
    public static string FieldKey => "PHOTO";

    Photo IV2FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);

        string? type = null, mimeType = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = data;
            // HACK: not sure how else to distinguish the type for v2.1
            else
                type = key;
        }

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(value, mimeType, encoding);

        return new Photo(finalValue, finalEncoding, type, finalMimeType);
    }

    Photo IV3FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, data) = DataSplitHelpers.SplitLine(FieldKey, input);

        string? type = null, mimeType = null, encoding = null, value = null;

        foreach (var datum in metadata)
        {
            var (key, entry) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = entry == "b" ? "BASE64" : entry;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                value = entry;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                type = entry;
        }

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(data, mimeType, encoding);

        return new Photo(finalValue, finalEncoding, type, finalMimeType, value);
    }

    Photo IV4FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        string? type = null, mimeType = null, encoding = null, valueMetadata = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeKey) ||
                key.EqualsIgnoreCase(FieldKeyConstants.MediaTypeAltKey))
                mimeType = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.ValueKey))
                valueMetadata = data;
            else if (key.EqualsIgnoreCase(FieldKeyConstants.TypeKey))
                type = data;
        }

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(value, mimeType, encoding);

        return new Photo(finalValue, finalEncoding, type, finalMimeType, valueMetadata);
    }

    private static (string Value, string? MimeType, string? Encoding) ParseDataUri(string value, string? mimeType,
        string? encoding)
    {
        const string dataPrefix = "data:";

        if (!value.StartsWithIgnoreCase(dataPrefix))
            return (value, mimeType, encoding);

        value = value.Substring(dataPrefix.Length);

        if (value.IndexOf(FieldKeyConstants.MetadataDelimiter) != -1)
        {
            var split = value.Split(FieldKeyConstants.MetadataDelimiter);
            if (split.Length > 1)
            {
                mimeType = split[0];
                value = split[1];
            }
        }

        if (value.IndexOf(FieldKeyConstants.ConcatenationDelimiter) != -1)
        {
            var split = value.Split(FieldKeyConstants.ConcatenationDelimiter);
            if (split.Length > 1)
            {
                encoding = split[0];
                value = split[1];
            }
        }

        return (value, mimeType, encoding);
    }
}