using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class OrganizationFieldDeserializer : IV2FieldDeserializer<Organization?>, IV3FieldDeserializer<Organization?>,
    IV4FieldDeserializer<Organization?>
{
    public static string FieldKey => "ORG";

    public Organization? Read(string input)
    {
        var replaceTarget = $"{FieldKey}:";
        var value = input.Replace(replaceTarget, string.Empty).Trim();
        string? orgName = null,
            orgUnitOne = null,
            orgUnitTwo = null;

        var parts = value.Split(';');
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