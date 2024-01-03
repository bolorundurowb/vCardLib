using System.Collections.Generic;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class CustomFieldDeserializer : IV2FieldDeserializer<KeyValuePair<string, string>>,
    IV3FieldDeserializer<KeyValuePair<string, string>>, IV4FieldDeserializer<KeyValuePair<string, string>>
{
    public static string Key => "UNKNOWN";

    public KeyValuePair<string, string> Read(string input)
    {
        var separatorIndex = input.LastIndexOf(':');
        var key = input.Substring(0, separatorIndex).Trim();
        var value = input.Substring(separatorIndex + 1).Trim();
        return new KeyValuePair<string, string>(key, value);
    }
}
