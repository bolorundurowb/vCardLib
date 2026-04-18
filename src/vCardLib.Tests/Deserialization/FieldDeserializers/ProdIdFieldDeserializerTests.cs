using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

public class ProdIdFieldDeserializerTests
{
    [Test]
    public void Read_V2Version_ReturnsCorrectValue()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV2FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }

    [Test]
    public void Read_V3Version_ReturnsCorrectValue()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV3FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }

    [Test]
    public void Read_V4Version_ReturnsCorrectValue()
    {
        const string input = "PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        IV4FieldDeserializer<string> deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }
}