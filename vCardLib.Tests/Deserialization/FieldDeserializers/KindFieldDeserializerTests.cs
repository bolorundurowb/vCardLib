using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class KindFieldDeserializerTests
{
    [Test]
    public void ShouldReturnNullForV2()
    {
        const string input = "KIND:individual";
        IV2FieldDeserializer<ContactKind?> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnNullForV3()
    {
        const string input = "KIND:individual";
        IV3FieldDeserializer<ContactKind?> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldParseIndividualKind()
    {
        const string input = "KIND:individual";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBe(ContactKind.Individual);
    }
    
    [Test]
    public void ShouldParseOrgKind()
    {
        const string input = "KIND:org";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBe(ContactKind.Organization);
    }
    
    [Test]
    public void ShouldParseLocationKind()
    {
        const string input = "KIND:location";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBe(ContactKind.Individual);
    }
    
    [Test]
    public void ShouldDefaultToIndividualKind()
    {
        const string input = "KIND:unknown";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldBe(ContactKind.Individual);
    }
}