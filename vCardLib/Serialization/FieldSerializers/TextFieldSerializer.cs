using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal abstract class TextFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>, IV4FieldSerializer<string>
{
    public abstract string FieldKey { get; }

    string IV2FieldSerializer<string>.Write(string data)
    {
        string value = data
            .Replace(@"\", @"\\")
            .Replace(";", @"\;")
            .Replace(",", @"\,");

        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{value}";
    }

    string IV3FieldSerializer<string>.Write(string data)
    {
        string value = data
            .Replace(@"\", @"\\")
            .Replace(";", @"\;")
            .Replace(",", @"\,")
            .Replace("\n", @"\n")
            .Replace("\r", @"\r");

        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{value}";
    }

    string IV4FieldSerializer<string>.Write(string data)
    {
        string value = data
            .Replace(@"\", @"\\")
            .Replace(";", @"\;")
            .Replace(",", @"\,")
            .Replace("\n", @"\n")
            .Replace("\r", @"\r")
            .Replace(@"""", @"\""");

        return $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{value}";
    }
}