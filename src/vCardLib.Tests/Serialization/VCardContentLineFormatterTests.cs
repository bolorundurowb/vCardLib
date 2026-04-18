using System.Linq;
using System.Text;
using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Tests.Serialization;

[TestFixture]
public class VCardContentLineFormatterTests
{
    [Test]
    public void AppendCrlf_UsesCrLfOnly()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendCrlf(sb, "BEGIN:VCARD");
        sb.ToString().ShouldBe("BEGIN:VCARD\r\n");
    }

    [Test]
    public void AppendFoldedContentLine_ShortLine_IsSinglePhysicalLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, "FN:Short");
        sb.ToString().ShouldBe("FN:Short\r\n");
    }

    [Test]
    public void AppendFoldedContentLine_Exactly75AsciiOctets_IsSinglePhysicalLine()
    {
        var payload = new string('a', 75);
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, payload);
        var line = sb.ToString();
        line.ShouldEndWith("\r\n");
        line.TrimEnd('\r', '\n').Length.ShouldBe(75);
    }

    [Test]
    public void AppendFoldedContentLine_76AsciiOctets_TwoPhysicalLinesSecondStartsWithSpace()
    {
        var payload = new string('b', 76);
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, payload);
        var text = sb.ToString();
        text.ShouldContain("\r\n ");
        var lines = text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        lines.Length.ShouldBe(2);
        lines[0].Length.ShouldBe(75);
        lines[1].ShouldStartWith(" ");
        (Encoding.UTF8.GetByteCount(lines[1]) <= 75).ShouldBeTrue();
    }

    [Test]
    public void AppendFoldedContentLine_DoesNotSplitUtf8Codepoint()
    {
        // U+1F600 GRINNING FACE is 4 UTF-8 bytes; place so a naive 75-octet cut would land inside it
        var prefix = new string('x', 72); // 72 octets
        var emoji = "\U0001F600"; // 4 octets; total logical line 76 octets
        var logical = prefix + emoji;
        Encoding.UTF8.GetByteCount(logical).ShouldBe(76);

        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, logical);
        var text = sb.ToString();

        var joined = string.Join(string.Empty, text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.StartsWith(" ", System.StringComparison.Ordinal) ? l.Substring(1) : l));
        joined.ShouldBe(logical);
    }

    [Test]
    public void AppendFoldedContentLine_EmptyString_EmitsBlankLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, string.Empty);
        sb.ToString().ShouldBe("\r\n");
    }
}
