using vCardLib.Constants;
using vCardLib.Models;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class UrlFieldSerializer : IV2FieldSerializer<Url>, IV3FieldSerializer<Url>,
    IV4FieldSerializer<Url>
{
    public string FieldKey => "URL";

    public string Write(Url data)
    {
        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";
    }
}
