using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class AgentFieldDeserializerTests
{
    [Test]
    public void ShouldReturnNullForV4()
    {
        const string input = "AGENT:http://mi6.gov.uk/007";
        IV4FieldDeserializer<string?> deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnValueForSimpleInput()
    {
        const string input = "AGENT:http://mi6.gov.uk/007";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldNotBeNull();
        result.ShouldBe("http://mi6.gov.uk/007");
    }
    
    [Test]
    public void ShouldReturnValueForValueTypeInput()
    {
        const string input = "AGENT;VALUE=uri:CID:JQPUBLIC.part3.960129T083020.xyzMail@host3.com";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldNotBeNull();
        result.ShouldBe("uri:CID:JQPUBLIC.part3.960129T083020.xyzMail@host3.com");
    }
    
    [Test]
    public void ShouldReturnValueForNestedInput()
    {
        const string input = @"AGENT:BEGIN:VCARD\nFN:Susan Thomas\nTEL:+1-919-555-1234\nEMAIL\;INTERNET:sthomas@host.com\nEND:VCARD\n";
        var deserializer = new AgentFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldNotBeNull();
        result.ShouldBe("BEGIN:VCARD\nFN:Susan Thomas\nTEL:+1-919-555-1234\nEMAIL;INTERNET:sthomas@host.com\nEND:VCARD\n");
    }
}