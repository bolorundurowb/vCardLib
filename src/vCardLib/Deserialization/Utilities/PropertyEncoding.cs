using System;
using System.Text;

namespace vCardLib.Deserialization.Utilities;

internal static class PropertyEncoding
{
    public static string? Parameter(string line, string name)
    {
        var colon = line.IndexOf(':');
        if (colon < 0) return null;
        foreach (var parameter in line.Substring(0, colon).Split(';'))
        {
            var equals = parameter.IndexOf('=');
            if (equals > 0 && parameter.Substring(0, equals).Equals(name, StringComparison.OrdinalIgnoreCase))
                return parameter.Substring(equals + 1).Trim('"');
        }
        return null;
    }

    public static Encoding Charset(string line)
    {
        var charset = Parameter(line, "CHARSET");
        if (charset == null) return Encoding.UTF8;
        return CodePagesEncodingProvider.Instance.GetEncoding(charset) ?? Encoding.GetEncoding(charset);
    }

    public static string DecodeValue(string line, string value) =>
        string.Equals(Parameter(line, "ENCODING"), "QUOTED-PRINTABLE", StringComparison.OrdinalIgnoreCase)
            ? SharedParsers.DecodeQuotedPrintable(value, Charset(line))
            : value;
}
