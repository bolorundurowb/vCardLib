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
    public void V2ShouldParseRawValue()
    {
        const string input = @"NOTE:Value with \n and \,";
        IV2FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(@"Value with \n and \,");
    }

    [Test]
    public void V3ShouldHandleEscapes()
    {
        const string input = @"NOTE:Line 1\nLine 2\\Comma\,Semi\;";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe($"Line 1{Environment.NewLine}Line 2\\Comma,Semi;");
    }

    [Test]
    public void V4ShouldHandleEscapes()
    {
        const string input = @"NOTE:Line 1\NLine 2\\Comma\,Semi\;";
        IV4FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe($"Line 1{Environment.NewLine}Line 2\\Comma,Semi;");
    }

    [Test]
    public void ShouldPreserveUnknownEscapes()
    {
        const string input = @"NOTE:Unknown \z escape";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(@"Unknown \z escape");
    }

    [Test]
    public void ShouldHandleEmptyValue()
    {
        const string input = "NOTE:";
        IV3FieldDeserializer<string> deserializer = new TestTextFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(string.Empty);
    }
}
