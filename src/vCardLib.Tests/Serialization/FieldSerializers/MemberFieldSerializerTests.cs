using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class MemberFieldSerializerTests
{
    [Test]
    public void FieldKey_ReturnsMember()
    {
        var serializer = new MemberFieldSerializer();
        serializer.FieldKey.ShouldBe("MEMBER");
    }

    [Test]
    public void Write_V2_ReturnsNull()
    {
        IV2FieldSerializer<string> serializer = new MemberFieldSerializer();
        var result = serializer.Write("mailto:subscriber@example.com");

        result.ShouldBeNull();
    }

    [Test]
    public void Write_V3_ReturnsNull()
    {
        IV3FieldSerializer<string> serializer = new MemberFieldSerializer();
        var result = serializer.Write("mailto:subscriber@example.com");

        result.ShouldBeNull();
    }

    [Test]
    public void Write_V4_ReturnsCorrectString()
    {
        IV4FieldSerializer<string> serializer = new MemberFieldSerializer();
        var result = serializer.Write("mailto:subscriber@example.com");

        result.ShouldBe("MEMBER:mailto:subscriber@example.com");
    }

    [Test]
    public void Write_V4_UrnMember_ReturnsCorrectString()
    {
        IV4FieldSerializer<string> serializer = new MemberFieldSerializer();
        var result = serializer.Write("urn:uuid:550e8400-e29b-41d4-a716-446655440000");

        result.ShouldBe("MEMBER:urn:uuid:550e8400-e29b-41d4-a716-446655440000");
    }

    [Test]
    public void Write_V4_HttpUri_ReturnsCorrectString()
    {
        IV4FieldSerializer<string> serializer = new MemberFieldSerializer();
        var result = serializer.Write("http://directory.example.com/pdir/jdoe");

        result.ShouldBe("MEMBER:http://directory.example.com/pdir/jdoe");
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        const string member = "mailto:subscriber1@example.com";
        IV4FieldSerializer<string> serializer = new MemberFieldSerializer();
        IV4FieldDeserializer<string> deserializer = new MemberFieldDeserializer();

        var wire = serializer.Write(member)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.ShouldBe(member);
    }
}
