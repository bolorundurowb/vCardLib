using System.Text.RegularExpressions;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class ProdIdFieldDeserializer : IV2FieldDeserializer<string>,
    IV3FieldDeserializer<string>, IV4FieldDeserializer<string>
{
    public static string FieldKey => "PRODID";

    public string Read(string input)
    {
        var separatorIndex = input.IndexOf(':');
        var value = input.Substring(separatorIndex + 1);
        return Regex.Unescape(value);
    }
}