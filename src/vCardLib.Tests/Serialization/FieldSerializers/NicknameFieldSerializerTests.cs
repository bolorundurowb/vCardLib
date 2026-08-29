using NUnit.Framework;
using OmniAssert;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class NicknameFieldSerializerTests
{
    [Test]
    public void Write_ValidNickname_ReturnsExpectedWireFormat()
    {
        var serializer = new NicknameFieldSerializer();
        var result = serializer.Write("Johnny");

        result.Must().Be("NICKNAME:Johnny");
    }

    [Test]
    public void Write_V2_ReturnsNull()
    {
        var serializer = new NicknameFieldSerializer();
        var result = (serializer as IV2FieldSerializer<string>).Write("Johnny");

        result.Must().BeNull();
    }
}
