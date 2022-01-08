using System;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class VersionDeserializer : IFieldDeserializer, IV2FieldDeserializer<vCardVersion>,
    IV3FieldDeserializer<vCardVersion>, IV4FieldDeserializer<vCardVersion>
{
    public string FieldKey => "VERSION";

    public vCardVersion Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
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