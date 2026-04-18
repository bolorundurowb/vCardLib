using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TextFieldDeserializerTests
{
    private class TestTextFieldDeserializer : TextFieldDeserializer
    {
    }

    [Test]
    public void Read_V2RawValue_ReturnsCorrectValue()
    {
        const string input = @"NOTE:Value with \n and \,";
        IV2FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(@"Value with \n and \,");
    }

    [Test]
    public void Read_V3Escapes_ReturnsCorrectValue()
    {
        const string input = @"NOTE:Line 1\nLine 2\\Comma\,Semi\;";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe($"Line 1{Environment.NewLine}Line 2\\Comma,Semi;");
    }

    [Test]
    public void Read_V4Escapes_ReturnsCorrectValue()
    {
        const string input = @"NOTE:Line 1\NLine 2\\Comma\,Semi\;";
        IV4FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe($"Line 1{Environment.NewLine}Line 2\\Comma,Semi;");
    }

    [Test]
    public void Read_UnknownEscapes_PreservesEscapes()
    {
        const string input = @"NOTE:Unknown \z escape";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(@"Unknown \z escape");
    }

    [Test]
    public void Read_EmptyValue_ReturnsEmptyString()
    {
        const string input = "NOTE:";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(string.Empty);
    }
}
