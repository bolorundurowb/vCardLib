using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NoteFieldDeserializerTests
{
    [Test]
    public void ShouldReturnParsedValue()
    {
        const string input = @"NOTE:This fax number is operational 0800 to 1715 EST\, Mon-Fri.";
        var deserializer = new NoteFieldDeserializer();
        var result = deserializer.Read(input);
        
        result.ShouldNotBeNull();
        result.ShouldBe(@"This fax number is operational 0800 to 1715 EST, Mon-Fri.");
    }
}