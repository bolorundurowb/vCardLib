using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

public class KeyFieldDeserializer : IV2FieldDeserializer<Key>, IV3FieldDeserializer<Key>,
    IV4FieldDeserializer<Key>
{
    public static string FieldKey => "KEY";

    Key IV2FieldDeserializer<Key>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        string? type = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = DataSplitHelpers.SplitDatum(datum, '=');

            if (key.EqualsIgnoreCase(FieldKeyConstants.EncodingKey))
                encoding = data;
            // HACK: not sure how else to distinguish the type for v2.1
            else
                type = key;
        }

        return new Key(value, type, null, encoding);
    }

    Key IV3FieldDeserializer<Key>.Read(string input)
    {
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var encoding = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.EncodingKey);
        if (encoding == "b") encoding = "BASE64";
        var type = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.TypeKey);

        return new Key(value, type, null, encoding);
    }

    Key IV4FieldDeserializer<Key>.Read(string input)
    {
        const string dataPrefix = "data:";
        var (metadata, value) = DataSplitHelpers.SplitLine(FieldKey, input);
        var parameters = VCardParameters.Parse(metadata);

        var mimeType = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.MediaTypeKey);
        var type = ParameterInterpreters.ParseStringParameter(parameters, FieldKeyConstants.TypeKey);
        string? encoding = null;

        if (value.StartsWithIgnoreCase(dataPrefix))
            value = value.Replace(dataPrefix, string.Empty);

        if (value.Contains(";"))
        {
            var split = value.Split(FieldKeyConstants.MetadataDelimiter);
            mimeType = split[0];
            value = split[1];
        }

        if (value.Contains(","))
        {
            var split = value.Split(FieldKeyConstants.ConcatenationDelimiter);
            encoding = split[0];
            value = split[1];
        }

        return new Key(value, type, mimeType, encoding);
    }
}
