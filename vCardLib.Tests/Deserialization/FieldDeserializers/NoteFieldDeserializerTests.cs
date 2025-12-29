using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NoteFieldDeserializerTests
{
    [Test]
    public void Write_Should_SerializeV2()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV2FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(@"This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Write_Should_SerializeV3()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV3FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(@"This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Write_Should_SerializeV4()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(@"This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }
}