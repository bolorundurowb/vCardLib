using System.IO;
using System.Text;

namespace vCardLib.Deserialization.Utilities;

internal static class FileDataHelpers
{
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
