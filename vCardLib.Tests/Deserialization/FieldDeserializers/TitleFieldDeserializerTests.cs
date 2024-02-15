using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class TitleFieldDeserializerTests
{
    [Test]
    public void ShouldReturnParsedValue()
    {
        const string input = "TITLE:Research Scientist";
        var deserializer = new TitleFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("Research Scientist");
    }
}