using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

public class ProdIdFieldDeserializerTests
{
    [Test]
    public void Write_Should_SerializeV2()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV2FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }

    [Test]
    public void Write_Should_SerializeV3()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV3FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }

    [Test]
    public void Write_Should_SerializeV4()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV4FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }
}