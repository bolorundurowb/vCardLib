using System;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

public class KeyFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Key>, IV3FieldDeserializer<Key>,
    IV4FieldDeserializer<Key>
{
    public string FieldKey => "KEY";

    Key IV2FieldDeserializer<Key>.Read(string input)
    {
        var (metadata, value) = Split(input);

        if (metadata.Length == 0)
            return new Key(value);

        string? type = null, mimeType = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = SplitDatum(datum);

            if (key.EqualsIgnoreCase("ENCODING"))
                encoding = data;
            // HACK: not sure how else to distinguish the type for v2.1
            else
                type = key;
        }

        return new Key(value, type, mimeType, encoding);
    }

    Key IV3FieldDeserializer<Key>.Read(string input)
    {
        var (metadata, value) = Split(input);

        if (metadata.Length == 0)
            return new Key(value);

        string? type = null, mimeType = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = SplitDatum(datum);

            if (key.EqualsIgnoreCase("ENCODING"))
                encoding = data == "b" ? "BASE64" : data;
            else if (key.EqualsIgnoreCase("TYPE"))
                type = data;
        }

        return new Key(value, type, mimeType, encoding);
    }

    Key IV4FieldDeserializer<Key>.Read(string input)
    {
        const string dataPrefix = "data:";
        var (metadata, value) = Split(input);

        if (metadata.Length == 0)
            return new Key(value);

        string? type = null, mimeType = null, encoding = null;

        foreach (var datum in metadata)
        {
            var (key, data) = SplitDatum(datum);

            if (key.EqualsIgnoreCase("MEDIATYPE"))
                mimeType = data;
        }

        if (value.StartsWithIgnoreCase(dataPrefix))
            value = value.Replace(dataPrefix, string.Empty);

        if (value.Contains(";"))
        {
            var split = value.Split(';');
            mimeType = split[0];
            value = split[1];
        }

        if (value.Contains(","))
        {
            var split = value.Split(',');
            encoding = split[0];
            value = split[1];
        }

        return new Key(value, type, mimeType, encoding);
    }

    private (string[], string) Split(string input)
    {
        input = input.Replace(FieldKey, string.Empty)
            .TrimStart(':')
            .TrimStart(';');

        if (Uri.IsWellFormedUriString(input, UriKind.Absolute))
            return (Array.Empty<string>(), input);

        var index = input.IndexOf(':');
        var metadata = input.Substring(0, index);
        var value = input.Substring(index + 1);

        return (metadata.Split(';'), value);
    }

    private static (string, string?) SplitDatum(string datum)
    {
        var parts = datum.Split('=');
        return parts.Length == 1 ? (parts[0], null) : (parts[0], parts[1]);
    }
}