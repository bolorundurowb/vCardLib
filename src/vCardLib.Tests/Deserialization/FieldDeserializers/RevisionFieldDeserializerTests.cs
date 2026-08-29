using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class RevisionFieldDeserializerTests
{
    [Test]
    public void Read_V2_ReturnsExpectedValue()
    {
        const string input = "REV:19951031T222710Z";
        IV2FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Year.Must().Be(1995);
        result.Value.Month.Must().Be(10);
        result.Value.Day.Must().Be(31);
    }

    [Test]
    public void Read_V3_ReturnsExpectedValue()
    {
        const string input = "REV:19951031T222710Z";
        IV3FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Year.Must().Be(1995);
    }

    [Test]
    public void Read_V4_ReturnsExpectedValue()
    {
        const string input = "REV:19951031T222710Z";
        IV4FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Value.Year.Must().Be(1995);
    }
}
