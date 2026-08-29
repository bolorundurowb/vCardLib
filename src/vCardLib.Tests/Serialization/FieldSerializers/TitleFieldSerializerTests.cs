using NUnit.Framework;
using OmniAssert;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TitleFieldSerializerTests
{
    private const string Title = "Web & UI/UX Designer";

    [Test]
    public void Write_V2_ReturnsExpectedWireFormat()
    {
        IV2FieldSerializer<string> serializer = new TitleFieldSerializer();
        serializer.Write(Title).Must().Be("TITLE:Web & UI/UX Designer");
    }

    [Test]
    public void Write_V3_ReturnsExpectedWireFormat()
    {
        IV3FieldSerializer<string> serializer = new TitleFieldSerializer();
        serializer.Write(Title).Must().Be("TITLE:Web & UI/UX Designer");
    }

    [Test]
    public void Write_V4_ReturnsExpectedWireFormat()
    {
        IV4FieldSerializer<string> serializer = new TitleFieldSerializer();
        serializer.Write(Title).Must().Be("TITLE:Web & UI/UX Designer");
    }
}
