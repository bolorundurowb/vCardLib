using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class NoteFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "NOTE";

    public string? Write(string data)
    {
        var value = Regex.Escape(data);
        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{value}";
    }
}
