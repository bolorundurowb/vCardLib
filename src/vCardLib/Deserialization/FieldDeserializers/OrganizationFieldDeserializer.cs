using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class OrganizationFieldDeserializer : IV2FieldDeserializer<Organization?>, IV3FieldDeserializer<Organization?>,
    IV4FieldDeserializer<Organization?>
{
    public static string FieldKey => "ORG";

    public Organization? Read(string input)
    {
        var colonIndex = input.IndexOf(':');
        var value = colonIndex >= 0 ? input.Substring(colonIndex + 1).Trim() : input.Trim();
        string? orgName = null,
            orgUnitOne = null,
            orgUnitTwo = null;

        var parts = value.Split(FieldKeyConstants.MetadataDelimiter);
        var partsLength = parts.Length;

        if (partsLength == 0)
            return null;

        orgName = Regex.Unescape(parts[0]);

        if (partsLength > 1)
            orgUnitOne = parts[1];

        if (partsLength > 2)
            orgUnitTwo = parts[2];

        return new Organization(orgName, orgUnitOne, orgUnitTwo);
    }
}