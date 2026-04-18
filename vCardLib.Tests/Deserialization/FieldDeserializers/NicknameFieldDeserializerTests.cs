using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NicknameFieldDeserializerTests
{
    [Test]
    public void Read_ValidInput_ReturnsCorrectValue()
    {
        var input = "NICKNAME:Johnny";
        var deserializer = new NicknameFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe("Johnny");
    }

    [Test]
    public void Read_V2Version_ReturnsNull()
    {
        var input = "NICKNAME:Johnny";
        var deserializer = new NicknameFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<string?>).Read(input);

        result.ShouldBeNull();
    }
}
