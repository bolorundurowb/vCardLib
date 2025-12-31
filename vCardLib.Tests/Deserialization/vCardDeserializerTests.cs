using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization;

namespace vCardLib.Tests.Deserialization;

[TestFixture]
public class vCardDeserializerTests
{
    [Test]
    public void FromContent_EmptyContent_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => vCardDeserializer.FromContent("").ToList());
        exception.Message.ShouldContain("File is empty.");
    }

    [Test]
    public void FromContent_MissingStartToken_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("VERSION:2.1\nEND:VCARD").ToList());
        exception.Message.ShouldContain("A vCard must begin with 'BEGIN:VCARD'");
    }

    [Test]
    public void FromContent_MissingEndToken_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("BEGIN:VCARD\nVERSION:2.1").ToList());
        exception.Message.ShouldContain("A vCard must end with 'END:VCARD'");
    }

    [Test]
    public void FromContent_MissingVersionKey_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("BEGIN:VCARD\nEND:VCARD").ToList());
        exception.Message.ShouldContain("A vCard must contain a 'VERSION'");
    }

    [Test]
    public void FromContent_ValidV21_ReturnsVCard()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:John Doe\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
        vcards[0].Version.ShouldBe(vCardLib.Enums.vCardVersion.v2);
    }

    [Test]
    public void FromContent_ValidV30_ReturnsVCard()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:John Doe\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
        vcards[0].Version.ShouldBe(vCardLib.Enums.vCardVersion.v3);
    }

    [Test]
    public void FromContent_ValidV40_ReturnsVCard()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:John Doe\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
        vcards[0].Version.ShouldBe(vCardLib.Enums.vCardVersion.v4);
    }

    [Test]
    public void FromFile_InvalidPath_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => vCardDeserializer.FromFile(""))
            .Message.ShouldContain("File path cannot be null or empty.");
    }

    [Test]
    public void FromFile_NonExistentFile_ThrowsFileNotFoundException()
    {
        Should.Throw<FileNotFoundException>(() => vCardDeserializer.FromFile("non-existent-file.vcf"));
    }

    [Test]
    public void FromStream_ValidStream_ReturnsVCard()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nFN:John Doe\nEND:VCARD";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));
        var vcards = vCardDeserializer.FromStream(stream).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
    }
}
