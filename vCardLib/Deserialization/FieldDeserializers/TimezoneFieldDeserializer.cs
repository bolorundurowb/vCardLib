using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class TimezoneFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string>
{
    public string FieldKey => "TZ";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        return input.Substring(separatorIndex + 1).Trim();
    }
}