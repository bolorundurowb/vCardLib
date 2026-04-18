using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class LabelFieldSerializer : IV2FieldSerializer<Label>, IV3FieldSerializer<Label>,
    IV4FieldSerializer<Label>
{
    public string FieldKey => "LABEL";

    public string? Write(Label data)
    {
        var builder = new StringBuilder(FieldKey);

        if (data.Type != AddressType.None)
        {
            foreach (var typeToken in data.Type.DecomposeAddressTypes())
            {
                builder.Append(FieldKeyConstants.MetadataDelimiter);
                builder.AppendFormat("{0}={1}", FieldKeyConstants.TypeKey, typeToken);
            }
        }

        builder.Append(FieldKeyConstants.SectionDelimiter);
        builder.Append(data.Text);

        return builder.ToString();
    }

    string? IV4FieldSerializer<Label>.Write(Label data) => null;
}
