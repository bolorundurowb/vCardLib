using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class ProdIdFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public string FieldKey => "PRODID";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1).Trim().ToLowerInvariant();
        return Regex.Unescape(value);
    }
}