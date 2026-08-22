using System.Collections.Generic;
using System.Linq;
using vCardLib.Constants;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Deserialization.FieldDeserializers;

internal sealed class CategoriesFieldDeserializer : IV2FieldDeserializer<List<string>>,
    IV3FieldDeserializer<List<string>>, IV4FieldDeserializer<List<string>>
{
    public static string FieldKey => "CATEGORIES";

    // v2.1 has no backslash escaping, so split on the raw delimiter.
    List<string> IV2FieldDeserializer<List<string>>.Read(string input) => Parse(input, unescape: false);

    public List<string> Read(string input) => Parse(input, unescape: true);

    private static List<string> Parse(string input, bool unescape)
    {
        var colonIndex = input.IndexOf(':');
        var value = colonIndex >= 0 ? input.Substring(colonIndex + 1) : input;

        if (string.IsNullOrWhiteSpace(value))
            return [];

        var parts = unescape
            ? ValueUnescaper.SplitUnescaped(value, FieldKeyConstants.ConcatenationDelimiter)
            : value.Split(FieldKeyConstants.ConcatenationDelimiter);

        return parts
            .Select(x => (unescape ? ValueUnescaper.Unescape(x) : x).Trim())
            .ToList();
    }
}
