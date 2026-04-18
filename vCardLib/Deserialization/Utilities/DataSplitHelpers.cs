using System;
using System.Collections.Generic;
using System.Text;
using vCardLib.Constants;

namespace vCardLib.Deserialization.Utilities;

internal static class DataSplitHelpers
{
    public static (string[] Parameters, string Value) SplitLine(string fieldKey, string input)
    {
        input = input.Trim();
        var colonIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        if (colonIndex == -1) 
            return ([], input);

        var prefix = input.Substring(0, colonIndex);
        var value = input.Substring(colonIndex + 1);

        var firstSemiColon = prefix.IndexOf(FieldKeyConstants.MetadataDelimiter);
        if (firstSemiColon == -1) 
            return ([], value);

        var metadata = prefix.Substring(firstSemiColon + 1);

        // Simple split by semicolon, but avoiding splitting inside quotes
        var parameters = new List<string>();
        var currentParam = new StringBuilder();
        var inQuotes = false;
        foreach (var c in metadata)
        {
            if (c == '"') inQuotes = !inQuotes;
            if (c == FieldKeyConstants.MetadataDelimiter && !inQuotes)
            {
                parameters.Add(currentParam.ToString());
                currentParam.Clear();
            }
            else
            {
                currentParam.Append(c);
            }
        }
        
        parameters.Add(currentParam.ToString());
        return (parameters.ToArray(), value);
    }

    public static IEnumerable<(string? Key, string Value)> ParseParameters(string[] parameters)
    {
        foreach (var param in parameters)
        {
            var (key, value) = ExtractKeyValue(param, '=');
            if (key == null)
            {
                yield return (null, value.Trim('"'));
            }
            else
            {
                // RFC 6350: TYPE=home,work -> should be treated as multiple type values
                if (key.Equals(FieldKeyConstants.TypeKey, StringComparison.OrdinalIgnoreCase))
                {
                    var values = value.Trim('"').Split(FieldKeyConstants.ConcatenationDelimiter);
                    foreach (var v in values)
                    {
                        yield return (FieldKeyConstants.TypeKey, v);
                    }
                }
                else
                {
                    yield return (key, value.Trim('"'));
                }
            }
        }
    }

    public static (string, string?) SplitDatum(string datum, char metadataSeparator)
    {
        var parts = datum.Split(metadataSeparator);
        return parts.Length == 1 ? (parts[0], null) : (parts[0], parts[1].Trim('"'));
    }

    // if there is no key, then it most likely a TYPE value
    public static (string? Key, string Value) ExtractKeyValue(string metadata, char metadataSeparator)
    {
        metadata = metadata.Trim();
        var separatorIndex = metadata.IndexOf(metadataSeparator);

        if (separatorIndex == -1)
            return (null, metadata.Trim());

        if (separatorIndex == 0 || separatorIndex == metadata.Length - 1)
            return (null, metadata.Trim(metadataSeparator));

        var key = metadata.Substring(0, separatorIndex).Trim();
        var value = metadata.Substring(separatorIndex + 1).Trim();
        return (key, value);
    }
}