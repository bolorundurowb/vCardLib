using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Deserialization.FieldDeserializers;

internal class LanguageFieldDeserializer : IFieldDeserializer, IV2FieldDeserializer<Language>, IV3FieldDeserializer<Language>,
    IV4FieldDeserializer<Language>
{
    public string FieldKey => "LANG";

    public Language Read(string input)
    {
        var index = input.IndexOf(':');
        var values = input.Substring(index + 1).Trim();
        var parts = values.Split(',');
        var latitude = float.Parse(parts[0]);
        var longitude = float.Parse(parts[1]);
        return new Geo(latitude, longitude);
    }
}