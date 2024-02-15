using System.Collections.Generic;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class CustomFieldSerializer : IV2FieldSerializer<KeyValuePair<string, string>>,
    IV3FieldSerializer<KeyValuePair<string, string>>,
    IV4FieldSerializer<KeyValuePair<string, string>>
{
    public string FieldKey => "UNKNOWN";

    public string? Write(KeyValuePair<string, string> data) => $"{data.Key}: {data.Value}";
}
