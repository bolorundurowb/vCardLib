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
    public void Parse_ShouldParse_LowercaseInput()
    {
        var value = "blue";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.ShouldBe(Color.Blue);
    }
    
    [Test]
    public void Parse_ShouldParse_UppercaseInput()
    {
        var value = "BLUE";

        var actual = EnumExtensions.Parse<Color>(value);

        actual.ShouldBe(Color.Blue);
    }

    [Test]
    public void Parse_InvalidInput_ThrowsArgumentException()
    {
        var value = "InvalidColor";

        Assert.Throws<ArgumentException>(() => EnumExtensions.Parse<Color>(value));
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

public enum Color
{
    Red = 1,
    Green = 2,
    Blue = 4
}