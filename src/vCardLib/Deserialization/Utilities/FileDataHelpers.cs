using System.IO;
using System.Text;

namespace vCardLib.Deserialization.Utilities;

internal static class FileDataHelpers
{
    public static string ReadContent(byte[] bytes)
    {
        using var stream = new MemoryStream(bytes);
        var encoding = stream.GetEncoding();
        if (encoding != Encoding.ASCII)
        {
            using var reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }

        // Inspect ASCII property headers before decoding their values, so that
        // different properties may declare different legacy character sets.
        var result = new StringBuilder();
        var currentEncoding = Encoding.UTF8;
        var softBreak = false;
        for (var start = 0; start < bytes.Length;)
        {
            var end = start;
            while (end < bytes.Length && bytes[end] != 10 && bytes[end] != 13) end++;
            var header = Encoding.ASCII.GetString(bytes, start, end - start);
            if (!softBreak && (header.Length == 0 || (header[0] != ' ' && header[0] != '\t')))
                currentEncoding = PropertyEncoding.Charset(header);
            result.Append(currentEncoding.GetString(bytes, start, end - start));
            softBreak = (softBreak || string.Equals(PropertyEncoding.Parameter(header, "ENCODING"),
                "QUOTED-PRINTABLE", System.StringComparison.OrdinalIgnoreCase)) && header.EndsWith("=");
            if (end < bytes.Length && bytes[end] == 13) end++;
            if (end < bytes.Length && bytes[end] == 10) end++;
            result.Append('\n');
            start = end;
        }
        return result.ToString();
    }

    public static Encoding GetEncoding(this Stream stream)
    {
        var bom = new byte[4];
        stream.Read(bom, 0, 4);

        stream.Position = 0;

        if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
            return Encoding.UTF7;

        if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            return Encoding.UTF8;

        if (bom[0] == 0xff && bom[1] == 0xfe)
            return Encoding.Unicode;

        if (bom[0] == 0xfe && bom[1] == 0xff)
            return Encoding.BigEndianUnicode;

        if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
            return Encoding.UTF32;

        return Encoding.ASCII;
    }
}
