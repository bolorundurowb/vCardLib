using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class NicknameFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "NICKNAME";

    // RFC-2425 does not support nicknames.
    string? IV2FieldSerializer<string>.Write(string data) => null;

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";
}
