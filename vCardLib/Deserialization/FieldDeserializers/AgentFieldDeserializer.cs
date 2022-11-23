using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class AgentFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string?>
{
    public string FieldKey => "AGENT";

    public string Read(string input)
    {
        input = input.Replace(FieldKey, string.Empty);

        // since the separator can be a ';' or ':' we need to trim them
        input = input.TrimStart(';').TrimStart(':');

        const string valuePreamble = "VALUE=";
        if (input.StartsWith(valuePreamble))
            input = input.Replace(valuePreamble, string.Empty);

        return Regex.Unescape(input);
    }

    string? IV4FieldDeserializer<string?>.Read(string input) => null;
}