using System.Text;

namespace vCardLib.Serialization.Utilities;

/// <summary>
/// RFC 2425 / RFC 2426 / RFC 6350 line delimiting and folding for <strong>serialization</strong> only:
/// CRLF line endings and 75-octet UTF-8 folding with space continuation.
/// </summary>
internal static class VCardContentLineFormatter
{
    public const string CrLf = "\r\n";

    /// <summary>Appends a single logical line (no folding) plus CRLF.</summary>
    public static void AppendCrlf(StringBuilder sb, string? line)
    {
        sb.Append(line ?? string.Empty);
        sb.Append(CrLf);
    }

    /// <summary>
    /// Appends one logical content line as one or more physical lines (max <paramref name="maxOctets"/>
    /// UTF-8 octets per physical line excluding CRLF; continuation lines begin with one space).
    /// </summary>
    public static void AppendFoldedContentLine(StringBuilder sb, string? logicalLine, int maxOctets = 75)
    {
        logicalLine ??= string.Empty;

        var bytes = Encoding.UTF8.GetBytes(logicalLine);
        if (bytes.Length == 0)
        {
            sb.Append(CrLf);
            return;
        }

        var offset = 0;
        var isFirstPhysicalLine = true;

        while (offset < bytes.Length)
        {
            var octetBudget = isFirstPhysicalLine ? maxOctets : maxOctets - 1; // leading space on continuations
            var segmentEnd = FindUtf8CutEnd(bytes, offset, octetBudget);
            if (segmentEnd == offset)
                segmentEnd = Utf8NextCodeUnitEnd(bytes, offset);

            if (!isFirstPhysicalLine)
                sb.Append(' ');

            sb.Append(Encoding.UTF8.GetString(bytes, offset, segmentEnd - offset));
            sb.Append(CrLf);

            offset = segmentEnd;
            isFirstPhysicalLine = false;
        }
    }

    /// <summary>Largest end index such that [start, end) is valid UTF-8 and (end - start) &lt;= budget.</summary>
    private static int FindUtf8CutEnd(byte[] bytes, int start, int budget)
    {
        if (budget <= 0)
            return start;

        var end = start + budget;
        if (end > bytes.Length)
            end = bytes.Length;

        while (end > start && end < bytes.Length && (bytes[end] & 0xC0) == 0x80)
            end--;

        return end;
    }

    /// <summary>Exclusive end index of the UTF-8 codepoint starting at <paramref name="start"/>.</summary>
    private static int Utf8NextCodeUnitEnd(byte[] bytes, int start)
    {
        if (start >= bytes.Length)
            return start;

        var b = bytes[start];
        int charLen;
        if ((b & 0x80) == 0)
            charLen = 1;
        else if ((b & 0xE0) == 0xC0)
            charLen = 2;
        else if ((b & 0xF0) == 0xE0)
            charLen = 3;
        else if ((b & 0xF8) == 0xF0)
            charLen = 4;
        else
            charLen = 1;

        return start + charLen > bytes.Length ? bytes.Length : start + charLen;
    }
}
