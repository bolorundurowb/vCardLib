using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class FormattedNameDeserializer : IV2FieldDeserializer<string>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "FN";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        return PropertyEncoding.DecodeValue(input, input.Substring(separatorIndex + 1).Trim());
    }
}
