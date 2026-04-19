using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Constants;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class DataSplitHelpersTests
{
    [Test]
    public void SplitLine_NoColon_ReturnsEmptyParametersAndFullInput()
    {
        var (parameters, value) = DataSplitHelpers.SplitLine("FN", "no colon here");
        parameters.ShouldBeEmpty();
        value.ShouldBe("no colon here");
    }

    [Test]
    public void SplitLine_NoMetadataSemicolon_ReturnsEmptyParameters()
    {
        var (parameters, value) = DataSplitHelpers.SplitLine("FN", "FN:John Doe");
        parameters.ShouldBeEmpty();
        value.ShouldBe("John Doe");
    }

    [Test]
    public void SplitLine_WithQuotedSemicolon_DoesNotSplitInsideQuotes()
    {
        var line = "ADR;TYPE=home;LABEL=\"a;b\";X=y:;;s1;s2;s3;s4;s5;s6;s7";
        var (parameters, value) = DataSplitHelpers.SplitLine("ADR", line);
        parameters.Length.ShouldBeGreaterThan(0);
        value.ShouldStartWith(";");
    }

    [Test]
    public void ParseParameters_TypeWithComma_YieldsMultipleTypeEntries()
    {
        var parameters = new[] { "TYPE=home,work" };
        var list = DataSplitHelpers.ParseParameters(parameters).ToList();
        list.Count.ShouldBe(2);
        list.All(x => x.Key == FieldKeyConstants.TypeKey).ShouldBeTrue();
        list[0].Value.ShouldBe("home");
        list[1].Value.ShouldBe("work");
    }

    [Test]
    public void ParseParameters_BareValue_YieldsNullKey()
    {
        var parameters = new[] { "internet" };
        var entry = DataSplitHelpers.ParseParameters(parameters).Single();
        entry.Key.ShouldBeNull();
        entry.Value.ShouldBe("internet");
    }

    [Test]
    public void ExtractKeyValue_NoSeparator_ReturnsNullKeyAndTrimmedMetadata()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("  bare  ", '=');
        key.ShouldBeNull();
        value.ShouldBe("bare");
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtStart_ReturnsNullKeyAndTrimmed()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("=valueonly", '=');
        key.ShouldBeNull();
        value.ShouldBe("valueonly");
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtEnd_ReturnsNullKey()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("KEY=", '=');
        key.ShouldBeNull();
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtStart_ReturnsNullKey()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("=onlyvalue", '=');
        key.ShouldBeNull();
    }

    [Test]
    public void SplitDatum_SinglePart_ReturnsNullSecond()
    {
        var (a, b) = DataSplitHelpers.SplitDatum("only", ';');
        a.ShouldBe("only");
        b.ShouldBeNull();
    }

    [Test]
    public void SplitDatum_TwoParts_ReturnsBoth()
    {
        var (a, b) = DataSplitHelpers.SplitDatum("a;b", ';');
        a.ShouldBe("a");
        b.ShouldBe("b");
    }
}
