using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NoteFieldDeserializerTests
{

    [Test]
    public void Read_V3_ReturnsExpectedValue()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV3FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Must().Be("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Read_V4_ReturnsExpectedValue()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Must().Be("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Read_V4_EncodingQuotedPrintable_DecodesBody()
    {
        const string input = "NOTE;ENCODING=QUOTED-PRINTABLE:=48=69";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).Must().Be("Hi");
    }

    [Test]
    public void Read_V4_EscapedNewline_ReplacesWithEnvironmentNewline()
    {
        var input = @"NOTE:line1\nline2";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);
        result.Must().Contain(System.Environment.NewLine);
        result.Must().Contain("line1");
        result.Must().Contain("line2");
    }

    [Test]
    public void Read_V4_EscapedSemicolonAndComma_Unescapes()
    {
        const string input = @"NOTE:a\;b\,c";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).Must().Be("a;b,c");
    }

    [Test]
    public void Read_V4_InvalidEscape_PreservesBackslash()
    {
        const string input = @"NOTE:keep\zhere";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).Must().Be(@"keep\zhere");
    }

    [Test]
    public void Read_V2_NoEscapeProcessing_ReturnsRawValue()
    {
        const string input = @"NOTE:raw\;value";
        IV2FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        deserializer.Read(input).Must().Be(@"raw\;value");
    }
}