using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NoteFieldDeserializerTests
{

    [Test]
    public void Read_V3Version_ReturnsCorrectValue()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV3FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Read_V4Version_ReturnsCorrectValue()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Read_V4_EncodingQuotedPrintable_DecodesBody()
    {
        const string input = "NOTE;ENCODING=QUOTED-PRINTABLE:=48=69";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).ShouldBe("Hi");
    }

    [Test]
    public void Read_V4_EscapedNewline_ReplacesWithEnvironmentNewline()
    {
        var input = @"NOTE:line1\nline2";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);
        result.ShouldContain(System.Environment.NewLine);
        result.ShouldContain("line1");
        result.ShouldContain("line2");
    }

    [Test]
    public void Read_V4_EscapedSemicolonAndComma_Unescapes()
    {
        const string input = @"NOTE:a\;b\,c";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).ShouldBe("a;b,c");
    }

    [Test]
    public void Read_V4_InvalidEscape_PreservesBackslash()
    {
        const string input = @"NOTE:keep\zhere";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).ShouldBe(@"keep\zhere");
    }

    [Test]
    public void Read_V2_NoEscapeProcessing_ReturnsRawValue()
    {
        const string input = @"NOTE:raw\;value";
        IV2FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).ShouldBe(@"raw\;value");
    }
}