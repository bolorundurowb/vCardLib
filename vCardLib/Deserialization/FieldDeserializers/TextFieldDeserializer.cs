using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal abstract class TextFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    string IV4FieldDeserializer<string>.Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();

        return value
            .Replace(@"\,", ",")
            .Replace(@"\;", ";")
            .Replace(@"\\", @"\");
    }

    string IV3FieldDeserializer<string>.Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();

        return value
            .Replace(@"\r", "\r")
            .Replace(@"\n", "\n")
            .Replace(@"\,", ",")
            .Replace(@"\;", ";")
            .Replace(@"\\", @"\");
    }

    string IV2FieldDeserializer<string>.Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();

        return value
            .Replace(@"\""", @"""")
            .Replace(@"\r", "\r")
            .Replace(@"\n", "\n")
            .Replace(@"\,", ",")
            .Replace(@"\;", ";")
            .Replace(@"\\", @"\");
    }
}