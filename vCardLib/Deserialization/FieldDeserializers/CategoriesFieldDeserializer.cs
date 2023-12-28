using System.Collections.Generic;
using System.Linq;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class CategoriesFieldDeserializer :  IV2FieldDeserializer<List<string>>,
    IV3FieldDeserializer<List<string>>, IV4FieldDeserializer<List<string>>
{
    public static string FieldKey => "CATEGORIES";

    public List<string> Read(string input)
    {
        input = input.ToUpper().Replace(FieldKey, string.Empty);
        var value = input.TrimStart(':');

        if (string.IsNullOrWhiteSpace(value))
            return new List<string>();

        return value.Split(',')
            .Select(x => x.Trim())
            .ToList();
    }
}