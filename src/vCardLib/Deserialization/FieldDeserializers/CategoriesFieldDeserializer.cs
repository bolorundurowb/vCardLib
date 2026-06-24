using System.Collections.Generic;
using System.Linq;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class CategoriesFieldDeserializer : IV2FieldDeserializer<List<string>>,
    IV3FieldDeserializer<List<string>>, IV4FieldDeserializer<List<string>>
{
    public static string FieldKey => "CATEGORIES";

    public List<string> Read(string input)
    {
        var colonIndex = input.IndexOf(':');
        var value = colonIndex >= 0 ? input.Substring(colonIndex + 1) : input;

        if (string.IsNullOrWhiteSpace(value))
            return [];

        return value.Split(FieldKeyConstants.ConcatenationDelimiter)
            .Select(x => x.Trim())
            .ToList();
    }
}