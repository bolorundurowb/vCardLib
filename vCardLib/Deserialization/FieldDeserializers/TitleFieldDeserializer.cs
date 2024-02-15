using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class TitleFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "TITLE";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();
        return Regex.Unescape(value);
    }
}