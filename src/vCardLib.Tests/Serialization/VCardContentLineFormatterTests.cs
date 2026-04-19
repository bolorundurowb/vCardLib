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

    [Test]
    public void AppendCrlf_EmptyString_EmitsOnlyLineBreak()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendCrlf(sb, string.Empty);
        sb.ToString().ShouldBe("\r\n");
    }

    [Test]
    public void AppendCrlf_NullTreatedAsEmptyLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendCrlf(sb, null);
        sb.ToString().ShouldBe("\r\n");
    }

    [Test]
    public void AppendCrlf_AppendsAfterExistingContent()
    {
        var sb = new StringBuilder("X");
        VCardContentLineFormatter.AppendCrlf(sb, "Y");
        sb.ToString().ShouldBe("XY\r\n");
    }

    [Test]
    public void CrLf_Constant_IsCarriageReturnLineFeed()
    {
        VCardContentLineFormatter.CrLf.ShouldBe("\r\n");
        VCardContentLineFormatter.CrLf.Length.ShouldBe(2);
    }

    [Test]
    public void AppendFoldedContentLine_NullTreatedAsEmptyLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, null);
        sb.ToString().ShouldBe("\r\n");
    }

    [Test]
    public void AppendFoldedContentLine_LongAsciiProducesMultiplePhysicalLines()
    {
        var payload = new string('c', 200);
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, payload);
        var text = sb.ToString();

        var physical = text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        (physical.Length >= 3).ShouldBeTrue();
        foreach (var line in physical)
            (Encoding.UTF8.GetByteCount(line) <= 75).ShouldBeTrue();

        var unfolded = string.Concat(physical.Select(l =>
            l.StartsWith(" ", System.StringComparison.Ordinal) ? l.Substring(1) : l));
        unfolded.ShouldBe(payload);
    }

    [Test]
    public void AppendFoldedContentLine_CustomMaxOctets_RespectsBudget()
    {
        var payload = new string('d', 11);
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, payload, maxOctets: 10);
        var physical = sb.ToString().Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        physical.Length.ShouldBe(2);
        (Encoding.UTF8.GetByteCount(physical[0]) <= 10).ShouldBeTrue();
        (Encoding.UTF8.GetByteCount(physical[1]) <= 10).ShouldBeTrue();
    }

    [Test]
    public void AppendFoldedContentLine_TwoByteUtf8AtBoundary_RejoinsCorrectly()
    {
        // U+00E9 LATIN SMALL LETTER E WITH ACUTE — 2 UTF-8 bytes (C3 A9)
        var prefix = new string('p', 74); // 74 octets
        var logical = prefix + "\u00e9";
        Encoding.UTF8.GetByteCount(logical).ShouldBe(76);

        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, logical);
        var text = sb.ToString();

        var joined = string.Concat(text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.StartsWith(" ", System.StringComparison.Ordinal) ? l.Substring(1) : l));
        joined.ShouldBe(logical);
    }

    [Test]
    public void AppendFoldedContentLine_ThreeByteUtf8AtBoundary_RejoinsCorrectly()
    {
        // U+3042 HIRAGANA LETTER A — 3 UTF-8 bytes
        var prefix = new string('q', 73); // 73 octets
        var logical = prefix + "\u3042";
        Encoding.UTF8.GetByteCount(logical).ShouldBe(76);

        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, logical);
        var text = sb.ToString();

        var joined = string.Concat(text.Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.StartsWith(" ", System.StringComparison.Ordinal) ? l.Substring(1) : l));
        joined.ShouldBe(logical);
    }

    [Test]
    public void AppendFoldedContentLine_MaxOctetsTwo_EachPhysicalLineAtMostTwoOctets()
    {
        var payload = "abcde";
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, payload, maxOctets: 2);
        var physical = sb.ToString().Split(new[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        (physical.Length > 1).ShouldBeTrue();
        foreach (var line in physical)
            (Encoding.UTF8.GetByteCount(line) <= 2).ShouldBeTrue();

        var unfolded = string.Concat(physical.Select(l =>
            l.StartsWith(" ", System.StringComparison.Ordinal) ? l.Substring(1) : l));
        unfolded.ShouldBe(payload);
    }

    [Test]
    public void AppendFoldedContentLine_MaxOctetsOne_SingleAsciiChar_OnePhysicalLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, "x", maxOctets: 1);
        sb.ToString().ShouldBe("x\r\n");
    }

    [Test]
    public void AppendFoldedContentLine_ChainedCalls_BuildsMultipleLines()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendCrlf(sb, "A");
        VCardContentLineFormatter.AppendFoldedContentLine(sb, "BB");
        VCardContentLineFormatter.AppendCrlf(sb, "C");
        sb.ToString().ShouldBe("A\r\nBB\r\nC\r\n");
    }

    [Test]
    public void AppendFoldedContentLine_SingleCharUtf8_FitsOnePhysicalLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, "\u3042");
        sb.ToString().ShouldBe("\u3042\r\n");
    }

    /// <summary>
    /// Forces FindUtf8CutEnd to shrink to start of a multi-byte sequence so Utf8NextCodeUnitEnd runs.
    /// </summary>
    [Test]
    public void AppendFoldedContentLine_SingleFourByteEmojiWithMaxOctetsOne_OnePhysicalLine()
    {
        var sb = new StringBuilder();
        VCardContentLineFormatter.AppendFoldedContentLine(sb, "\U0001F600", maxOctets: 1);
        sb.ToString().ShouldBe("\U0001F600\r\n");
    }
}
