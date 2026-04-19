using NUnit.Framework;
using Shouldly;
using vCardLib.Extensions;

namespace vCardLib.Tests.Extensions;

[TestFixture]
public class StringExtensionsTests
{
    [TestCase("hello", "HELLO", true)]
    [TestCase("Hello", "hello", true)]
    [TestCase("a", "b", false)]
    public void EqualsIgnoreCase_ReturnsExpected(string input, string value, bool expected)
    {
        input.EqualsIgnoreCase(value).ShouldBe(expected);
    }

    [TestCase("BEGIN:VCARD", "begin:", true)]
    [TestCase("VERSION:4.0", "version", true)]
    [TestCase("FN:X", "gz:", false)]
    public void StartsWithIgnoreCase_ReturnsExpected(string input, string value, bool expected)
    {
        input.StartsWithIgnoreCase(value).ShouldBe(expected);
    }

    [TestCase("file.TXT", ".txt", true)]
    [TestCase("name.vcf", ".VCF", true)]
    [TestCase("a.b", ".c", false)]
    public void EndsWithIgnoreCase_ReturnsExpected(string input, string value, bool expected)
    {
        input.EndsWithIgnoreCase(value).ShouldBe(expected);
    }
}
