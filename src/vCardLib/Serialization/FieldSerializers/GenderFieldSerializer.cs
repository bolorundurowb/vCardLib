using System.Text;
using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class GenderFieldSerializer : IV2FieldSerializer<Gender>, IV3FieldSerializer<Gender>,
    IV4FieldSerializer<Gender>
{
    public string FieldKey => "GENDER";

    public string? Write(Gender data) => null;

    string? IV4FieldSerializer<Gender>.Write(Gender data)
    {
        var builder = new StringBuilder(FieldKey);
        builder.Append(FieldKeyConstants.SectionDelimiter);

        if (data.Sex.HasValue)
            builder.Append(data.Sex.Value.DecomposeBiologicalSex());

        if (data.GenderIdentity != null)
        {
            builder.Append(FieldKeyConstants.MetadataDelimiter);
            builder.Append(data.GenderIdentity);
        }

        return builder.ToString();
    }
}
