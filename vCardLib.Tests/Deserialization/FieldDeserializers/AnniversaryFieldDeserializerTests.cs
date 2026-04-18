using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class AnniversaryFieldDeserializerTests
{
    [Test]
    public void Read_V2Version_ReturnsNull()
    {
        const string input = "ANNIVERSARY:19901021";
        IV2FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_V3Version_ReturnsNull()
    {
        const string input = "ANNIVERSARY:19901021";
        IV3FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_UnexpectedInput_ReturnsNull()
    {
        var input = string.Empty;
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_DateOnly_ReturnsCorrectValue()
    {
        const string input = "ANNIVERSARY:19901021";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(1990, 10, 21, 0, 0, 0, DateTimeKind.Utc));
    }

    [Test]
    public void Read_DateAndTime_ReturnsCorrectValue()
    {
        const string input = "ANNIVERSARY:20121201T134211Z";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(2012, 12, 1, 13, 42, 11, DateTimeKind.Utc));
    }

    [Test]
    public void Read_TimeOnly_ReturnsCorrectValue()
    {
        const string input = "ANNIVERSARY:0415";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        var timeSpan = new TimeSpan(4, 15, 00);

        result.ShouldNotBeNull();
        (result != null ? result.Value - DateTime.MinValue : (TimeSpan?)null).ShouldBe(timeSpan);
    }
}