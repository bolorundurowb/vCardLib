using NUnit.Framework;
using Shouldly;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class NameFieldSerializerTests
{
    [Test]
    public void Write_ValidName_ReturnsCorrectString()
    {
        var name = new Name("Doe", "John", "Middle", "Mr.", "Esq.");
        var serializer = new NameFieldSerializer();
        var result = serializer.Write(name);

        result.ShouldBe("N:Doe;John;Middle;Mr.;Esq.");
    }
}
