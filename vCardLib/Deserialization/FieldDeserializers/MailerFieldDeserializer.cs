using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class MailerFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string?>
{
    public string FieldKey => "MAILER";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        return input.Substring(separatorIndex + 1).Trim();
    }

    string? IV4FieldDeserializer<string?>.Read(string input) => null;
}