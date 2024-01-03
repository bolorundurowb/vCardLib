using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class NicknameFieldDeserializer : IV2FieldDeserializer<string?>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "NICKNAME";

    /// <summary>
    /// The nickname field is not supported on the v2 standard
    /// </summary>
    /// <returns>null</returns>
    string? IV2FieldDeserializer<string?>.Read(string input) => null;

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        return input.Substring(separatorIndex + 1).Trim();
    }
}