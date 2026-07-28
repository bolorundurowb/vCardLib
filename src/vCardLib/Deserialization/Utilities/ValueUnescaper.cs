using System;
using System.Collections.Generic;
using System.Text;

namespace vCardLib.Deserialization.Utilities;

internal static class ValueUnescaper
{
    /// <summary>
    /// Inverse of <see cref="Serialization.Utilities.ValueEscaper.Escape"/>: decodes
    /// \\, \, \; and \n / \N. Undefined escapes keep their backslash (RFC 6350 3.4).
    /// </summary>
    public static string Unescape(string source, bool handleNewlines = true)
    {
        if (string.IsNullOrEmpty(source) || !source.Contains("\\"))
            return source;

        var sb = new StringBuilder(source.Length);

        for (var i = 0; i < source.Length; i++)
        {
            var c = source[i];

            if (c == '\\' && i + 1 < source.Length)
            {
                var next = source[i + 1];

                if (handleNewlines && (next == 'n' || next == 'N'))
                {
                    sb.Append(Environment.NewLine);
                    i++;
                }
                else if (next == '\\' || next == ',' || next == ';')
                {
                    sb.Append(next);
                    i++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Splits on unescaped occurrences of <paramref name="delimiter"/> only, leaving escape
    /// sequences intact for a later <see cref="Unescape"/> pass.
    /// </summary>
    public static string[] SplitUnescaped(string value, char delimiter)
    {
        var result = new List<string>();
        var sb = new StringBuilder();

        for (var i = 0; i < value.Length; i++)
        {
            var c = value[i];

            if (c == '\\' && i + 1 < value.Length)
            {
                sb.Append(c);
                sb.Append(value[i + 1]);
                i++;
            }
            else if (c == delimiter)
            {
                result.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }

        result.Add(sb.ToString());
        return result.ToArray();
    }
}
