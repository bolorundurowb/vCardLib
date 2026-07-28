using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class OrganizationFieldDeserializer : IV2FieldDeserializer<Organization?>, IV3FieldDeserializer<Organization?>,
    IV4FieldDeserializer<Organization?>
{
    public static string FieldKey => "ORG";

    // v2.1 has no backslash escaping, so split on the raw delimiter.
    Organization? IV2FieldDeserializer<Organization?>.Read(string input) => Parse(input, unescape: false);

    public Organization? Read(string input) => Parse(input, unescape: true);

    private static Organization? Parse(string input, bool unescape)
    {
        var colonIndex = input.IndexOf(':');
        var value = colonIndex >= 0 ? input.Substring(colonIndex + 1).Trim() : input.Trim();

        var parts = unescape
            ? ValueUnescaper.SplitUnescaped(value, FieldKeyConstants.MetadataDelimiter)
            : value.Split(FieldKeyConstants.MetadataDelimiter);

        if (parts.Length == 0)
            return null;

        string Component(int index) => unescape ? ValueUnescaper.Unescape(parts[index]) : parts[index];

        var orgName = Component(0);
        var orgUnitOne = parts.Length > 1 ? Component(1) : null;
        var orgUnitTwo = parts.Length > 2 ? Component(2) : null;

        return new Organization(orgName, orgUnitOne, orgUnitTwo);
    }
}
