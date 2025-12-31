using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class NicknameFieldSerializerTests
{
    [Test]
    public void Write_ValidNickname_ReturnsCorrectString()
    {
        var serializer = new NicknameFieldSerializer();
        var result = serializer.Write("Johnny");

        result.ShouldBe("NICKNAME:Johnny");
    }

    [Test]
    public void Write_V2_ReturnsNull()
    {
        var serializer = new NicknameFieldSerializer();
        var result = (serializer as IV2FieldSerializer<string>).Write("Johnny");

        result.ShouldBeNull();
    }
}
