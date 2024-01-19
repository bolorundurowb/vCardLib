using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class MailerFieldDeserializer : IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string?>
{
    public static string FieldKey => "MAILER";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        return input.Substring(separatorIndex + 1).Trim();
    }

    string? IV4FieldDeserializer<string?>.Read(string input) => null;
}