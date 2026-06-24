using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class AgentFieldDeserializerTests
{
    [Test]
    public void Read_V4Version_ReturnsNull()
    {
        const string input = "AGENT:http://mi6.gov.uk/007";
        IV4FieldDeserializer<string?> deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_SimpleInput_ReturnsCorrectValue()
    {
        const string input = "AGENT:http://mi6.gov.uk/007";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("http://mi6.gov.uk/007");
    }

    [Test]
    public void Read_ValueTypeInput_ReturnsCorrectValue()
    {
        const string input = "AGENT;VALUE=uri:CID:JQPUBLIC.part3.960129T083020.xyzMail@host3.com";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("CID:JQPUBLIC.part3.960129T083020.xyzMail@host3.com");
    }

    [Test]
    public void Read_NestedInput_ReturnsCorrectValue()
    {
        const string input = @"AGENT:BEGIN:VCARD\nFN:Susan Thomas\nTEL:+1-919-555-1234\nEMAIL\;INTERNET:sthomas@host.com\nEND:VCARD\n";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("BEGIN:VCARD\nFN:Susan Thomas\nTEL:+1-919-555-1234\nEMAIL;INTERNET:sthomas@host.com\nEND:VCARD\n");
    }

    [Test]
    public void Read_EscapedComma_ReturnsUnescapedValue()
    {
        const string input = @"AGENT:ABC\, Inc.";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("ABC, Inc.");
    }

    [Test]
    public void Read_EscapedBackslash_ReturnsUnescapedValue()
    {
        const string input = @"AGENT:path\\to\\file";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("path\\to\\file");
    }

    [Test]
    public void Read_QuotedLabelWithColon_ReturnsCorrectValue()
    {
        const string input = @"AGENT;LABEL=""My:Label"":value";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("value");
    }

    [Test]
    public void Read_V2Version_ReturnsCorrectValue()
    {
        const string input = "AGENT:http://example.com/agent";
        IV2FieldDeserializer<string> deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("http://example.com/agent");
    }

    [Test]
    public void Read_V3Version_ReturnsCorrectValue()
    {
        const string input = "AGENT:http://example.com/agent";
        IV3FieldDeserializer<string> deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("http://example.com/agent");
    }

    [Test]
    public void Read_WithParameters_IgnoresParameters()
    {
        const string input = "AGENT;VALUE=uri;TYPE=work:http://example.com/agent";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("http://example.com/agent");
    }

    [Test]
    public void Read_WithEscapedNewline_UnescapesNewline()
    {
        const string input = @"AGENT:Line1\nLine2";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("Line1\nLine2");
    }

    [Test]
    public void Read_WithEscapedSemicolon_UnescapesSemicolon()
    {
        const string input = @"AGENT:value\;with\;semicolons";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("value;with;semicolons");
    }

    [Test]
    public void Read_WithUnknownEscape_PreservesBackslash()
    {
        const string input = @"AGENT:value\unknown";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("value\\unknown");
    }

    [Test]
    public void Read_WithTrailingBackslash_PreservesBackslash()
    {
        const string input = @"AGENT:value\";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("value\\");
    }
}