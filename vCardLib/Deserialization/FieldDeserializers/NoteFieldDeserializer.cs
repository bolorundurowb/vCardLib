using System.Text.RegularExpressions;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class NoteFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "NOTE";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();
        return Regex.Unescape(value);
    }
}