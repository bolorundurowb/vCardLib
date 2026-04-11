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
    public void Read_InputV2_ShouldReturnNull()
    {
        const string input = "ANNIVERSARY:19901021";
        IV2FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_InputV3_ShouldReturnNull()
    {
        const string input = "ANNIVERSARY:19901021";
        IV3FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_EmptyInput_ShouldReturnNull()
    {
        var input = string.Empty;
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void Read_DateOnlyInput_ShouldReturnValue()
    {
        const string input = "ANNIVERSARY:19901021";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(1990, 10, 21, 0, 0, 0, DateTimeKind.Utc));
    }

    [Test]
    public void Read_DateAndTimeInput_ShouldReturnValue()
    {
        const string input = "ANNIVERSARY:20121201T134211Z";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(2012, 12, 1, 13, 42, 11, DateTimeKind.Utc));
    }

    [Test]
    public void Read_TimeOnlyInput_ShouldReturnValue()
    {
        const string input = "ANNIVERSARY:0415";
        IV4FieldDeserializer<DateTime?> deserializer = new AnniversaryFieldDeserializer();
        var result = deserializer.Read(input);

        var timeSpan = new TimeSpan(4, 15, 00);

        result.ShouldNotBeNull();
        (result != null ? result.Value - DateTime.MinValue : (TimeSpan?)null).ShouldBe(timeSpan);
    }
}