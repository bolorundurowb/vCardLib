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
    public void Read_V2_ReturnsNull()
    {
        const string input = "KIND:individual";
        IV2FieldDeserializer<ContactKind?> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_V3_ReturnsNull()
    {
        const string input = "KIND:individual";
        IV3FieldDeserializer<ContactKind?> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_IndividualKind_ReturnsExpectedValue()
    {
        const string input = "KIND:individual";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(ContactKind.Individual);
    }

    [Test]
    public void Read_OrgKind_ReturnsExpectedValue()
    {
        const string input = "KIND:org";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(ContactKind.Organization);
    }

    [Test]
    public void Read_LocationKind_ReturnsExpectedValue()
    {
        const string input = "KIND:location";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(ContactKind.Location);
    }

    [Test]
    public void Read_UnknownKind_DefaultsToIndividual()
    {
        const string input = "KIND:unknown";
        IV4FieldDeserializer<ContactKind> deserializer = new KindFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBe(ContactKind.Individual);
    }
}