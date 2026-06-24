using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization;
using vCardLib.Enums;
using vCardLib.Tests.Helpers;

namespace vCardLib.Tests.Deserialization;

[TestFixture]
public class vCardDeserializerTests
{
    [Test]
    public void FromContent_WhenEmpty_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => vCardDeserializer.FromContent("").ToList());
        exception!.Message.ShouldContain("File is empty.");
    }

    [Test]
    public void FromContent_WhenNull_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(() => vCardDeserializer.FromContent(null!).ToList());
        exception!.Message.ShouldContain("File is empty.");
    }

    [Test]
    public void FromContent_WhenWhitespaceOnly_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => vCardDeserializer.FromContent("   \t  ").ToList())
            .Message.ShouldContain("File is empty.");
    }

    [Test]
    public void FromContent_WhenMissingStartToken_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("VERSION:2.1\nEND:VCARD").ToList());
        exception!.Message.ShouldContain("A vCard must begin with 'BEGIN:VCARD'");
    }

    [Test]
    public void FromContent_WhenMissingEndToken_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("BEGIN:VCARD\nVERSION:2.1").ToList());
        exception!.Message.ShouldContain("A vCard must end with 'END:VCARD'");
    }

    [Test]
    public void FromContent_WhenMissingVersionKey_ThrowsException()
    {
        var exception = Assert.Throws<Exception>(() => vCardDeserializer.FromContent("BEGIN:VCARD\nEND:VCARD").ToList());
        exception!.Message.ShouldContain("A vCard must contain a 'VERSION'");
    }

    [Test]
    public void FromContent_WhenLowercaseBeginToken_ThrowsException()
    {
        var content = "begin:vcard\nVERSION:3.0\nFN:Case\nEND:VCARD";

        Should.Throw<Exception>(() => vCardDeserializer.FromContent(content).ToList())
            .Message.ShouldContain("A vCard must begin with 'BEGIN:VCARD'");
    }

    [Test]
    public void FromContent_WhenLowercaseEndToken_ThrowsException()
    {
        var content = "BEGIN:VCARD\nVERSION:3.0\nFN:Case\nend:vcard";

        Should.Throw<Exception>(() => vCardDeserializer.FromContent(content).ToList())
            .Message.ShouldContain("A vCard must end with 'END:VCARD'");
    }

    [Test]
    public void FromContent_WhenUnsupportedVersion_ThrowsArgumentOutOfRangeException()
    {
        var content = "BEGIN:VCARD\nVERSION:1.0\nFN:X\nEND:VCARD";

        Should.Throw<ArgumentOutOfRangeException>(() => vCardDeserializer.FromContent(content).ToList());
    }

    [Test]
    public void FromContent_WhenVersionOnlyInFieldValue_ThrowsArgumentException()
    {
        var content = "BEGIN:VCARD\nFN:VERSION:2.1 embedded\nEND:VCARD";

        var exception = Assert.Throws<ArgumentException>(() => vCardDeserializer.FromContent(content).ToList());
        exception!.Message.ShouldContain("No version specified");
    }

    [TestCase(vCardVersion.v2)]
    [TestCase(vCardVersion.v3)]
    [TestCase(vCardVersion.v4)]
    public void FromContent_WhenValidVersion_ReturnsVCard(vCardVersion version)
    {
        var vcards = vCardDeserializer.FromContent(VCardTestContent.Minimal(version, "John Doe")).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
        vcards[0].Version.ShouldBe(version);
    }

    [Test]
    public void FromFile_WhenInvalidPath_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => vCardDeserializer.FromFile(""))
            .Message.ShouldContain("File path cannot be null or empty.");
    }

    [Test]
    public void FromFile_WhenWhitespacePath_ThrowsArgumentException()
    {
        Should.Throw<ArgumentException>(() => vCardDeserializer.FromFile("   "));
    }

    [Test]
    public void FromFile_WhenFileNotFound_ThrowsFileNotFoundException()
    {
        Should.Throw<FileNotFoundException>(() => vCardDeserializer.FromFile("non-existent-file.vcf"));
    }

    [Test]
    public void FromFile_WhenValidFile_ReturnsVCard()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"vcard-{Guid.NewGuid():N}.vcf");
        try
        {
            File.WriteAllText(tempPath, VCardTestContent.Minimal(vCardVersion.v4, "From File"));
            var vcards = vCardDeserializer.FromFile(tempPath).ToList();

            vcards.Count.ShouldBe(1);
            vcards[0].FormattedName.ShouldBe("From File");
        }
        finally
        {
            if (File.Exists(tempPath))
                File.Delete(tempPath);
        }
    }

    [Test]
    public void FromStream_WhenValidUtf8_ReturnsVCard()
    {
        var content = VCardTestContent.Minimal(vCardVersion.v2, "John Doe");
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        var vcards = vCardDeserializer.FromStream(stream).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("John Doe");
    }

    [Test]
    public void FromStream_WhenUtf8Bom_ReturnsVCard()
    {
        var content = VCardTestContent.Minimal(vCardVersion.v2, "Jane");
        var preamble = Encoding.UTF8.GetPreamble();
        var body = Encoding.UTF8.GetBytes(content);
        using var stream = new MemoryStream();
        stream.Write(preamble, 0, preamble.Length);
        stream.Write(body, 0, body.Length);
        stream.Position = 0;

        var vcards = vCardDeserializer.FromStream(stream).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("Jane");
    }

    [Test]
    public void FromStream_WhenUtf16LeBom_ReturnsVCard()
    {
        var content = VCardTestContent.Minimal(vCardVersion.v4, "Utf16");
        var body = Encoding.Unicode.GetBytes(content);
        var preamble = Encoding.Unicode.GetPreamble();
        using var stream = new MemoryStream();
        stream.Write(preamble, 0, preamble.Length);
        stream.Write(body, 0, body.Length);
        stream.Position = 0;

        var vcards = vCardDeserializer.FromStream(stream).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].FormattedName.ShouldBe("Utf16");
    }

    [Test]
    public void FromContent_WhenFoldedLineWithLeadingSpace_UnfoldsProperty()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nNOTE:This is a long note that continues\n on the next line.\nEND:VCARD";

        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(1);
        vcards[0].Note.ShouldNotBeNull();
        vcards[0].Note!.ShouldContain("This is a long note that continues");
        vcards[0].Note!.ShouldContain("on the next line.");
    }

    [Test]
    public void FromContent_WhenV2QuotedPrintableContinuation_UnfoldsProperty()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nNOTE:joined=\nwithout equals sign\nEND:VCARD";

        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards[0].Note.ShouldBe("joinedwithout equals sign");
    }

    [Test]
    public void FromContent_WhenCrlfFoldedNote_UnfoldsProperty()
    {
        var content = "BEGIN:VCARD\r\nVERSION:4.0\r\nNOTE:line1\r\n continuation\r\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].Note.ShouldBe("line1continuation");
    }

    [Test]
    public void FromContent_WhenTabContinuation_UnfoldsProperty()
    {
        var content = "BEGIN:VCARD\nVERSION:2.1\nNOTE:hello\n\tworld\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].Note.ShouldBe("helloworld");
    }

    [Test]
    public void FromContent_WhenMultipleCards_ReturnsAllCards()
    {
        var content =
            $"{VCardTestContent.Minimal(vCardVersion.v3, "One")}\n{VCardTestContent.Minimal(vCardVersion.v3, "Two")}";

        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(2);
        vcards[0].FormattedName.ShouldBe("One");
        vcards[1].FormattedName.ShouldBe("Two");
    }

    [Test]
    public void FromContent_WhenMultipleCardsWithDifferentVersions_ReturnsAllCards()
    {
        var content =
            $"{VCardTestContent.Minimal(vCardVersion.v2, "V2 Card")}\n{VCardTestContent.Minimal(vCardVersion.v4, "V4 Card")}";
        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(2);
        vcards[0].Version.ShouldBe(vCardVersion.v2);
        vcards[0].FormattedName.ShouldBe("V2 Card");
        vcards[1].Version.ShouldBe(vCardVersion.v4);
        vcards[1].FormattedName.ShouldBe("V4 Card");
    }

    [Test]
    public void FromContent_WhenLeadingUnicodeBom_Parses()
    {
        var content = "\uFEFF" + VCardTestContent.Minimal(vCardVersion.v4, "Bom");
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].FormattedName.ShouldBe("Bom");
    }

    [Test]
    public void FromContent_WhenCrOnlyLineEndings_Parses()
    {
        var content = VCardTestContent.Minimal(vCardVersion.v3, "CrOnly").Replace("\n", "\r");
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].FormattedName.ShouldBe("CrOnly");
    }

    [Test]
    public void FromContent_WhenMixedCrLfAndLf_Parses()
    {
        var content = "BEGIN:VCARD\r\nVERSION:3.0\nFN:Mixed\r\nEND:VCARD";
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].FormattedName.ShouldBe("Mixed");
    }

    [Test]
    public void FromContent_WhenLeadingBlankLinesBeforeBegin_Parses()
    {
        var content = "\n\n" + VCardTestContent.Minimal(vCardVersion.v4, "Leading");
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].FormattedName.ShouldBe("Leading");
    }

    [Test]
    public void FromContent_WhenTrailingBlankLinesAfterEnd_Parses()
    {
        var content = VCardTestContent.Minimal(vCardVersion.v4, "Trailing") + "\n\n";
        var vcards = vCardDeserializer.FromContent(content).ToList();
        vcards[0].FormattedName.ShouldBe("Trailing");
    }

    [Test]
    public void FromContent_WhenTrailingSpacesOnEndLine_ReturnsNoCards()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Padded\nEND:VCARD   \t";
        var vcards = vCardDeserializer.FromContent(content).ToList();

        vcards.Count.ShouldBe(0);
    }

    [Test]
    public void FromContent_WhenStrictCrlfWireFormat_ParsesFormattedName()
    {
        var wire = "BEGIN:VCARD\r\nVERSION:4.0\r\nFN:Strict\r\nEND:VCARD\r\n";
        var vcards = vCardDeserializer.FromContent(wire).ToList();
        vcards[0].FormattedName.ShouldBe("Strict");
    }

    [Test]
    public void FromContent_WhenV4EscapedSemicolonInNote_UnescapesValue()
    {
        var content = "BEGIN:VCARD\nVERSION:4.0\nFN:Escape\nNOTE:part1\\;part2\nEND:VCARD";
        var card = vCardDeserializer.FromContent(content).Single();

        card.Note.ShouldBe("part1;part2");
    }
}
