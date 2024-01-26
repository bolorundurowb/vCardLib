using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class RevisionFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "REV";

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";
}
