using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class MemberFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "MEMBER";

    public string? Write(string data) => null;

    string IV4FieldSerializer<string>.Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";
}
