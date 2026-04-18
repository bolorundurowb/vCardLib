using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Utilities;

namespace vCardLib.Tests.Utilities;

public class EnumExtensionsTests
{
    [Test]
    public void Parse_ValidInput_ReturnsCorrectEnum()
    {
        const string value = "Red";
        var expected = Color.Red;

        var actual = EnumExtensions.Parse<Color>(value);

        actual.ShouldBe(expected);
    }

    [Test]
    public void Parse_LowercaseInput_ReturnsCorrectEnum()
    {
        const string value = "blue";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.ShouldBe(Color.Blue);
    }

    [Test]
    public void Parse_UppercaseInput_ReturnsCorrectEnum()
    {
        const string value = "BLUE";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.ShouldBe(Color.Blue);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsArgumentException()
    {
        const string value = "InvalidColor";

        Assert.Throws<ArgumentException>(() => EnumExtensions.Parse<Color>(value));
    }

    [Test]
    public void Parse_NullInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => EnumExtensions.Parse<Color>(null!));
    }

    [Test]
    public void Parse_EmptyInput_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => EnumExtensions.Parse<Color>(string.Empty));
    }

    [Test]
    public void Values_OneFlagSet_ReturnsSingleValue()
    {
        var value = Color.Red;

        var actual = EnumExtensions.Values(value);

        actual.ShouldBe(new[] { Color.Red });
    }

    [Test]
    public void Values_MultipleFlagsSet_ReturnsMultipleValues()
    {
        var value = Color.Red | Color.Green;

        var actual = EnumExtensions.Values(value);

        actual.ShouldBe(new[] { Color.Red, Color.Green });
    }
}

[Flags]
public enum Color
{
    Red = 1,
    Green = 2,
    Blue = 4
}