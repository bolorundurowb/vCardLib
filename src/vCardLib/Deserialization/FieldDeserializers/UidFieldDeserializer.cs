using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class UidFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string>
{
    public static string FieldKey => "UID";

    public string Read(string input)
    {
        var colonIndex = input.IndexOf(':');
        return colonIndex >= 0 ? input.Substring(colonIndex + 1).Trim() : input.Trim();
    }
}