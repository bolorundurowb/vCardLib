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

    // v2.1 has no backslash escaping mechanism.
    string? IV2FieldSerializer<Label>.Write(Label data) => Format(data, escape: false);

    public string? Write(Label data) => Format(data, escape: true);

    string? IV4FieldSerializer<Label>.Write(Label data) => null;

    private string Format(Label data, bool escape)
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
        builder.Append(escape ? ValueEscaper.Escape(data.Text) : data.Text);

        return builder.ToString();
    }
}
