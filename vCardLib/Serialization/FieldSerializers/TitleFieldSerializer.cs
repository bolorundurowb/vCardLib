using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class TitleFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "TITLE";

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{Regex.Escape(data)}";
}
