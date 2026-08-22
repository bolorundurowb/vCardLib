using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class OrganizationFieldSerializer : IV2FieldSerializer<Organization>, IV3FieldSerializer<Organization>,
    IV4FieldSerializer<Organization>
{
    public string FieldKey => "ORG";

    // v2.1 has no backslash escaping mechanism.
    string IV2FieldSerializer<Organization>.Write(Organization data) => Format(data, escape: false);

    public string Write(Organization data) => Format(data, escape: true);

    private string Format(Organization data, bool escape)
    {
        string E(string? component) => escape ? ValueEscaper.Escape(component) : component ?? string.Empty;

        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(E(data.Name));

        if (!string.IsNullOrWhiteSpace(data.PrimaryUnit))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(E(data.PrimaryUnit));
        }

        if (!string.IsNullOrWhiteSpace(data.SecondaryUnit))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(E(data.SecondaryUnit));
        }

        return builder.ToString();
    }
}
