using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Utilities;

namespace vCardLib.Tests.Utilities;

public class EnumExtensionsTests
{
    [Test]
    public void Parse_ValidInput_ReturnsExpectedEnum()
    {
        const string value = "Red";
        var expected = Color.Red;

        var actual = EnumExtensions.Parse<Color>(value);

        actual.Must().Be(expected);
    }

    [Test]
    public void Parse_LowercaseInput_ReturnsExpectedEnum()
    {
        const string value = "blue";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.Must().Be(Color.Blue);
    }

    [Test]
    public void Parse_UppercaseInput_ReturnsExpectedEnum()
    {
        const string value = "BLUE";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.Must().Be(Color.Blue);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsArgumentException()
    {
        const string value = "InvalidColor";

        Ensure.Throws<ArgumentException>(() => EnumExtensions.Parse<Color>(value));
    }

    [Test]
    public void Parse_NullInput_ThrowsArgumentNullException()
    {
        Ensure.Throws<ArgumentNullException>(() => EnumExtensions.Parse<Color>(null!));
    }

    [Test]
    public void Parse_EmptyInput_ThrowsArgumentNullException()
    {
        Ensure.Throws<ArgumentNullException>(() => EnumExtensions.Parse<Color>(string.Empty));
    }

    [Test]
    public void Values_OneFlagSet_ReturnsSingleValue()
    {
        var value = Color.Red;

        var actual = EnumExtensions.Values(value);

        actual.Must().BeSequenceEqual(new[] { Color.Red });
    }

    [Test]
    public void Values_MultipleFlagsSet_ReturnsMultipleValues()
    {
        var value = Color.Red | Color.Green;

        var actual = EnumExtensions.Values(value);

        actual.Must().BeSequenceEqual(new[] { Color.Red, Color.Green });
    }
}

[Flags]
public enum Color
{
    Red = 1,
    Green = 2,
    Blue = 4
}