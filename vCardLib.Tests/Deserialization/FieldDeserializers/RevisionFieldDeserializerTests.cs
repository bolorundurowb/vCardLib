using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class RevisionFieldDeserializerTests
{
    [Test]
    public void ShouldParseRevisionV2()
    {
        const string input = "REV:19951031T222710Z";
        IV2FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Year.ShouldBe(1995);
        result.Value.Month.ShouldBe(10);
        result.Value.Day.ShouldBe(31);
    }

    [Test]
    public void ShouldParseRevisionV3()
    {
        const string input = "REV:19951031T222710Z";
        IV3FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Year.ShouldBe(1995);
    }

    [Test]
    public void ShouldParseRevisionV4()
    {
        const string input = "REV:19951031T222710Z";
        IV4FieldDeserializer<DateTime?> deserializer = new RevisionFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.Value.Year.ShouldBe(1995);
    }
}
