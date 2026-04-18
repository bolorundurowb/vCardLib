using vCardLib.Constants;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.FieldSerializers;

internal sealed class MailerFieldSerializer : IV2FieldSerializer<string>, IV3FieldSerializer<string>,
    IV4FieldSerializer<string>
{
    public string FieldKey => "MAILER";

    public string Write(string data) => $"{FieldKey}{FieldKeyConstants.SectionDelimiter}{data}";

    string? IV4FieldSerializer<string>.Write(string data) => null;
}
