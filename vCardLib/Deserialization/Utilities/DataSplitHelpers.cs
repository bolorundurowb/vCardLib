using System;
using vCardLib.Constants;

namespace vCardLib.Deserialization.Utilities;

internal static class DataSplitHelpers
{
    public static (string[], string) SplitLine(string fieldKey, string input)
    {
        var colonIndex = input.IndexOf(FieldKeyConstants.SectionDelimiter);
        var semiColonIndex = input.IndexOf(FieldKeyConstants.MetadataDelimiter);

        // if there is no metadata, the first separator should be a colon
        if (semiColonIndex == -1 || semiColonIndex > colonIndex)
        {
            var value = input.Substring(colonIndex + 1);
            return (Array.Empty<string>(), value);
        }

        var metadata = input.Substring(fieldKey.Length + 1, colonIndex - fieldKey.Length - 1);
        var remainder = input.Substring(colonIndex + 1);

        return (metadata.Split(FieldKeyConstants.MetadataDelimiter), remainder);
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