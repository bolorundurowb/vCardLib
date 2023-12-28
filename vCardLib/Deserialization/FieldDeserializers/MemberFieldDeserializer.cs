using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class MemberFieldDeserializer :  IV2FieldDeserializer<string?>,
    IV3FieldDeserializer<string?>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "MEMBER";

    public string? Read(string input) => null;

    string IV4FieldDeserializer<string>.Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        return input.Substring(separatorIndex + 1).Trim();
    }
}