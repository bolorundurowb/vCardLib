using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class ProdIdFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "PRODID";

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{Regex.Escape(data)}";
}
