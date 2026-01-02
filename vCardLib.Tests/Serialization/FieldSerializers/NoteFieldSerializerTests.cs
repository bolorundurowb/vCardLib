using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class NoteFieldSerializerTests
{
    [Test]
    public void Write_Note_ReturnsCorrectString()
    {
        var serializer = new NoteFieldSerializer();
        var result = ((vCardLib.Serialization.Interfaces.IV2FieldSerializer<string>)serializer).Write("This is a note");
        result.ShouldBe("NOTE:This is a note");
    }

    [Test]
    public void Write_NoteWithSpecialChars_EscapesInV3()
    {
        var serializer = new NoteFieldSerializer();
        var result = ((vCardLib.Serialization.Interfaces.IV3FieldSerializer<string>)serializer).Write("Note with; commas, and\\ backslashes");
        result.ShouldBe(@"NOTE:Note with\; commas\, and\\ backslashes");
    }
}
