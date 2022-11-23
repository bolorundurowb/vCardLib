using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class UrlFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<string>, IV3FieldDeserializer<string>,
    IV4FieldDeserializer<string>
{
    public string FieldKey => "URL";

    public string Read(string input)
    {
        var replaceTarget = $"{FieldKey}:";
        return input.Replace(replaceTarget, string.Empty).Trim();
    }
}