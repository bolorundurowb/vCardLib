using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NoteFieldDeserializerTests
{

    [Test]
    public void Read_V3Input_ShouldParseCorrectly()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV3FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }

    [Test]
    public void Read_V4Input_ShouldParseCorrectly()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        IV4FieldDeserializer<string> deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe("This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }
}