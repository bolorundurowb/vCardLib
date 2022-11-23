namespace vCardLib.Utilities;

internal static class DataSplitHelpers
{
    public static (string[], string) SplitLine(string fieldKey, string input)
    {
        var index = input.LastIndexOf(':');
        var metadata = input.Substring(0, index);
        var value = input.Substring(index + 1);

        metadata = metadata.Replace(fieldKey, string.Empty)
            .TrimStart(':')
            .TrimStart(';');

        return (metadata.Split(';'), value);
    }

    public static (string, string?) SplitDatum(string datum, char metadataSeparator)
    {
        var parts = datum.Split(metadataSeparator);
        return parts.Length == 1 ? (parts[0], null) : (parts[0], parts[1]);
    }
}
