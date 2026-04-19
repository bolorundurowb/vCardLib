using NUnit.Framework;
using Shouldly;
using vCardLib.Utilities;

namespace vCardLib.Tests.Utilities;

[TestFixture]
public class StringHelpersTests
{
    [TestCase("\"hello\"", true)]
    [TestCase("  \"hello\"  ", true)]
    [TestCase("\"\"", true)]
    [TestCase("\"a\"", true)]
    [TestCase("hello", false)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase("   ", false)]
    [TestCase("\"", false)]
    [TestCase("\"hello", false)]
    [TestCase("hello\"", false)]
    [TestCase("\"hello\"extra", false)]
    public void IsQuoted_ReturnsExpected(string? input, bool expected)
    {
        StringHelpers.IsQuoted(input).ShouldBe(expected);
    }

    [Test]
    public void IsQuoted_WhitespaceAroundQuotes_IsRecognized()
    {
        StringHelpers.IsQuoted(" \t \"quoted value\" \r\n ").ShouldBeTrue();
    }
}
