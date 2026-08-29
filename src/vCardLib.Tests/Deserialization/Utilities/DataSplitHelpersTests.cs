using System.Linq;
using NUnit.Framework;
using OmniAssert;
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
        parameters.Must().BeEmpty();
        value.Must().Be("no colon here");
    }

    [Test]
    public void SplitLine_NoMetadataSemicolon_ReturnsEmptyParameters()
    {
        var (parameters, value) = DataSplitHelpers.SplitLine("FN", "FN:John Doe");
        parameters.Must().BeEmpty();
        value.Must().Be("John Doe");
    }

    [Test]
    public void SplitLine_WithQuotedSemicolon_DoesNotSplitInsideQuotes()
    {
        var line = "ADR;TYPE=home;LABEL=\"a;b\";X=y:;;s1;s2;s3;s4;s5;s6;s7";
        var (parameters, value) = DataSplitHelpers.SplitLine("ADR", line);
        parameters.Length.Must().BeGreaterThan(0);
        value.Must().StartWith(";");
    }

    [Test]
    public void ParseParameters_TypeWithComma_YieldsMultipleTypeEntries()
    {
        var parameters = new[] { "TYPE=home,work" };
        var list = DataSplitHelpers.ParseParameters(parameters).ToList();
        list.Count.Must().Be(2);
        list.All(x => x.Key == FieldKeyConstants.TypeKey).Must().BeTrue();
        list[0].Value.Must().Be("home");
        list[1].Value.Must().Be("work");
    }

    [Test]
    public void ParseParameters_BareValue_YieldsNullKey()
    {
        var parameters = new[] { "internet" };
        var entry = DataSplitHelpers.ParseParameters(parameters).Single();
        entry.Key.Must().BeNull();
        entry.Value.Must().Be("internet");
    }

    [Test]
    public void ExtractKeyValue_NoSeparator_ReturnsNullKeyAndTrimmedMetadata()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("  bare  ", '=');
        key.Must().BeNull();
        value.Must().Be("bare");
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtStart_ReturnsNullKeyAndTrimmed()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("=valueonly", '=');
        key.Must().BeNull();
        value.Must().Be("valueonly");
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtEnd_ReturnsNullKey()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("KEY=", '=');
        key.Must().BeNull();
    }

    [Test]
    public void ExtractKeyValue_SeparatorAtStart_ReturnsNullKey()
    {
        var (key, value) = DataSplitHelpers.ExtractKeyValue("=onlyvalue", '=');
        key.Must().BeNull();
    }

    [Test]
    public void SplitDatum_SinglePart_ReturnsNullSecond()
    {
        var (a, b) = DataSplitHelpers.SplitDatum("only", ';');
        a.Must().Be("only");
        b.Must().BeNull();
    }

    [Test]
    public void SplitDatum_TwoParts_ReturnsBoth()
    {
        var (a, b) = DataSplitHelpers.SplitDatum("a;b", ';');
        a.Must().Be("a");
        b.Must().Be("b");
    }
}
