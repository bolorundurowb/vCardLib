using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class NameFieldDeserializer : IV2FieldDeserializer<Name>, IV3FieldDeserializer<Name>,
    IV4FieldDeserializer<Name>
{
    public static string FieldKey => "N";

    // v2.1 has no backslash escaping, so split on the raw delimiter.
    Name IV2FieldDeserializer<Name>.Read(string input) => Parse(input, unescape: false);

    public Name Read(string input) => Parse(input, unescape: true);

    private static Name Parse(string input, bool unescape)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();
        string? familyName = null,
            givenName = null,
            additionalNames = null,
            honorificPrefix = null,
            honorificSuffix = null;

        var parts = unescape
            ? ValueUnescaper.SplitUnescaped(value, FieldKeyConstants.MetadataDelimiter)
            : value.Split(FieldKeyConstants.MetadataDelimiter);

        string? Component(int index) =>
            unescape ? ValueUnescaper.Unescape(parts[index]) : parts[index];

        if (parts.Length > 0)
            familyName = Component(0);

        if (parts.Length > 1)
            givenName = Component(1);

        if (parts.Length > 2)
            additionalNames = Component(2);

        if (parts.Length > 3)
            honorificPrefix = Component(3);

        if (parts.Length > 4)
            honorificSuffix = Component(4);

        return new Name(familyName, givenName, additionalNames, honorificPrefix, honorificSuffix);
    }
}
