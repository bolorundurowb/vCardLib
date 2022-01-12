using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class BirthdayFieldDeserializerTests
{
    [Test]
    public void ShouldReturnNullWithUnexpectedInput()
    {
        var input = string.Empty;
        var deserializer = new BirthdayFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }

    [Test]
    public void ShouldReturnValueWithDateOnly()
    {
        const string input = "BDAY:19901021";
        var deserializer = new BirthdayFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(1990, 10, 21, 0, 0, 0, DateTimeKind.Utc));
    }

    [Test]
    public void ShouldReturnValueWithDateAndTime()
    {
        const string input = "BDAY;20121201T134211Z";
        var deserializer = new BirthdayFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(new DateTime(2012, 12, 1, 13, 42, 11, DateTimeKind.Utc));
    }

    [Test]
    public void ShouldReturnValueWithTimeOnly()
    {
        const string input = "BDAY:--0415";
        var deserializer = new BirthdayFieldDeserializer();
        var result = deserializer.Read(input);

        var timeSpan = new TimeSpan(4, 15, 00);

        result.ShouldNotBeNull();
        (result - DateTime.MinValue).ShouldBe(timeSpan);
    }
}