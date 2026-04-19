using System.IO;
using System.Text;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class FileDataHelpersTests
{
    private static Encoding ReadEncoding(byte[] preambleAndRest)
    {
        using var stream = new MemoryStream(preambleAndRest);
        return stream.GetEncoding();
    }

    [Test]
    public void GetEncoding_Utf8Bom_ReturnsUtf8()
    {
        var bytes = new byte[] { 0xEF, 0xBB, 0xBF, 0x41 };
        ReadEncoding(bytes).ShouldBe(Encoding.UTF8);
    }

    [Test]
    public void GetEncoding_Utf16LittleEndianBom_ReturnsUnicode()
    {
        var bytes = new byte[] { 0xFF, 0xFE, 0x41, 0x00 };
        ReadEncoding(bytes).ShouldBe(Encoding.Unicode);
    }

    [Test]
    public void GetEncoding_Utf16BigEndianBom_ReturnsBigEndianUnicode()
    {
        var bytes = new byte[] { 0xFE, 0xFF, 0x00, 0x41 };
        ReadEncoding(bytes).ShouldBe(Encoding.BigEndianUnicode);
    }

    [Test]
    public void GetEncoding_Utf32BeBom_ReturnsUtf32()
    {
        var bytes = new byte[] { 0x00, 0x00, 0xFE, 0xFF };
        ReadEncoding(bytes).ShouldBe(Encoding.UTF32);
    }

    [Test]
    public void GetEncoding_Utf7Bom_ReturnsUtf7()
    {
        var bytes = new byte[] { 0x2B, 0x2F, 0x76, 0x38 };
#pragma warning disable SYSLIB0001 // UTF-7 required to match FileDataHelpers implementation
        ReadEncoding(bytes).ShouldBe(Encoding.UTF7);
#pragma warning restore SYSLIB0001
    }

    [Test]
    public void GetEncoding_NoRecognizedBom_ReturnsAscii()
    {
        var bytes = new byte[] { 0x41, 0x42, 0x43, 0x44 };
        ReadEncoding(bytes).ShouldBe(Encoding.ASCII);
    }

    [Test]
    public void GetEncoding_ResetsStreamPosition()
    {
        var bytes = new byte[] { 0xEF, 0xBB, 0xBF, 0x48, 0x69 };
        using var stream = new MemoryStream(bytes);
        _ = stream.GetEncoding();
        stream.Position.ShouldBe(0);
    }
}
