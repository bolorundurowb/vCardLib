using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NicknameFieldDeserializerTests
{
    [Test]
    public void Read_ValidInput_ReturnsExpectedValue()
    {
        var input = "NICKNAME:Johnny";
        var deserializer = new NicknameFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().Be("Johnny");
    }

    [Test]
    public void Read_V2_ReturnsNull()
    {
        var input = "NICKNAME:Johnny";
        var deserializer = new NicknameFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<string?>).Read(input);

        result.Must().BeNull();
    }
}
