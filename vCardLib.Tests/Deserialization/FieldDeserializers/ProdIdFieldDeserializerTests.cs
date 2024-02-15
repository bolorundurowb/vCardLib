using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

public class ProdIdFieldDeserializerTests
{
    [Test]
    public void ShouldReturnParsedValue()
    {
        const string input = @"PRODID:-//ONLINE DIRECTORY//NONSGML Version 1//EN";
        var deserializer = new ProdIdFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(@"-//ONLINE DIRECTORY//NONSGML Version 1//EN");
    }
}