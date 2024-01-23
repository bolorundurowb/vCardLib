using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class OrganizationFieldSerializer : IV2FieldSerializer<Organization>, IV3FieldSerializer<Organization>,
    IV4FieldSerializer<Organization>
{
    public string FieldKey => "ORG";

    public string Write(Organization data)
    {
        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Name);

        if (!string.IsNullOrWhiteSpace(data.PrimaryUnit))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.PrimaryUnit);
        }

        if (!string.IsNullOrWhiteSpace(data.SecondaryUnit))
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.SecondaryUnit);
        }

        return builder.ToString();
    }
}
