using System;
using vCardLib.Constants;
using vCardLib.Enums;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class VersionDeserializer
{
    public static string FieldKey => "VERSION";

    public static vCardVersion Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var value = input.Substring(separatorIndex + 1).Trim();

        return value switch
        {
            "2.1" => vCardVersion.v2,
            "3.0" => vCardVersion.v3,
            "4.0" => vCardVersion.v4,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Parsed version is not supported.")
        };
    }
}