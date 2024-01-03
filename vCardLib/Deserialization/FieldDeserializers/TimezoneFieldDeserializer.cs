using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class TimezoneFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string>
{
    public static string FieldKey => "TZ";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        return input.Substring(separatorIndex + 1).Trim();
    }
}