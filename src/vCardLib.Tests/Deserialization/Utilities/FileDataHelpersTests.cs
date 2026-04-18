using System.IO;
using System.Text;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class FileDataHelpersTests
{
    [Test]
    public void GetEncoding_NoBom_ReturnsAsciiAndResetsPosition()
    {
        var bytes = Encoding.ASCII.GetBytes("BEGIN:VCARD");
        using var stream = new MemoryStream(bytes);

        var encoding = stream.GetEncoding();

        encoding.ShouldBe(Encoding.ASCII);
        stream.Position.ShouldBe(0);
    }

    [Test]
    public void GetEncoding_Utf8Bom_ReturnsUtf8()
    {
        var preamble = Encoding.UTF8.GetPreamble();
        var body = Encoding.UTF8.GetBytes("BEGIN:VCARD");
        using var stream = new MemoryStream();
        stream.Write(preamble, 0, preamble.Length);
        stream.Write(body, 0, body.Length);
        stream.Position = 0;

        stream.GetEncoding().ShouldBe(Encoding.UTF8);
        stream.Position.ShouldBe(0);
    }

    [Test]
    public void GetEncoding_Utf16LittleEndianBom_ReturnsUnicode()
    {
        var preamble = Encoding.Unicode.GetPreamble();
        using var stream = new MemoryStream();
        stream.Write(preamble, 0, preamble.Length);
        stream.Position = 0;

        stream.GetEncoding().ShouldBe(Encoding.Unicode);
        stream.Position.ShouldBe(0);
    }

    [Test]
    public void GetEncoding_Utf16BigEndianBom_ReturnsBigEndianUnicode()
    {
        var preamble = Encoding.BigEndianUnicode.GetPreamble();
        using var stream = new MemoryStream();
        stream.Write(preamble, 0, preamble.Length);
        stream.Position = 0;

        stream.GetEncoding().ShouldBe(Encoding.BigEndianUnicode);
        stream.Position.ShouldBe(0);
    }

    [Test]
    public void GetEncoding_Utf7Bom_ReturnsUtf7()
    {
        // UTF-7 BOM sequence used by the library (+/v)
        using var stream = new MemoryStream(new byte[] { 0x2b, 0x2f, 0x76, 0x38 });

#pragma warning disable SYSLIB0001
        stream.GetEncoding().ShouldBe(Encoding.UTF7);
#pragma warning restore SYSLIB0001
        stream.Position.ShouldBe(0);
    }

    [Test]
    public void GetEncoding_Utf32Bom_ReturnsUtf32()
    {
        using var stream = new MemoryStream(new byte[] { 0, 0, 0xfe, 0xff });

        stream.GetEncoding().ShouldBe(Encoding.UTF32);
        stream.Position.ShouldBe(0);
    }
}
