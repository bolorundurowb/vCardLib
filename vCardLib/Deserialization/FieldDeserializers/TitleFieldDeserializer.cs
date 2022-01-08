using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class TitleFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public string FieldKey => "TITLE";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1).Trim();
        return Regex.Unescape(value);
    }
}