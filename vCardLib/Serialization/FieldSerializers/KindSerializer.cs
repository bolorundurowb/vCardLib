using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class KindSerializer : IV2FieldSerializer<ContactKind>, IV3FieldSerializer<ContactKind>,
    IV4FieldSerializer<ContactKind>
{
    public string FieldKey => "KIND";

    public string? Write(ContactKind data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data.DecomposeContactKind()}";
}
