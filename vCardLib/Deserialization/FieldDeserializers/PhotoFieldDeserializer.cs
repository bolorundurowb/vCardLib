using System;
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
        
        string? type = null, encoding = null;
        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');
            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = data;
            else
                type = key;
        }

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(value, null, encoding);

        return new Photo(finalValue, finalEncoding, type, finalMimeType);
    }

    Photo IV3FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, data) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var encoding = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.EncodingKey);
        if (encoding == "b") encoding = "BASE64";

        var type = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.TypeKey);
        var valueMetadata = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.ValueKey);

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(data, null, encoding);

        return new Photo(finalValue, finalEncoding, type, finalMimeType, valueMetadata);
    }

    Photo IV4FieldDeserializer<Photo>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var mimeType = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.MediaTypeKey)
                       ?? ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.MediaTypeAltKey);
        var type = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.TypeKey);
        var valueMetadata = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.ValueKey);

        var (finalValue, finalMimeType, finalEncoding) = ParseDataUri(value, mimeType, null);

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