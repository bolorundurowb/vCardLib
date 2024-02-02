using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class UrlFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "URL";

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";
}
