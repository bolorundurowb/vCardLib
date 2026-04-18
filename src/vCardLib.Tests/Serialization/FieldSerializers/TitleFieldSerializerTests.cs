using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class TitleFieldSerializerTests
{
    [Test]
    public void Write_Should_SerializeV2()
    {
        const string data = "Web & UI/UX Designer";
        IV2FieldSerializer<string> serializer = new TitleFieldSerializer();
        var result = serializer.Write(data);

        result.ShouldNotBeNull();
        result.ShouldBe("TITLE:Web & UI/UX Designer");
    }

    [Test]
    public void Write_Should_SerializeV3()
    {
        const string data = "Web & UI/UX Designer";
        IV3FieldSerializer<string> serializer = new TitleFieldSerializer();
        var result = serializer.Write(data);

        result.ShouldNotBeNull();
        result.ShouldBe("TITLE:Web & UI/UX Designer");
    }

    [Test]
    public void Write_Should_SerializeV4()
    {
        const string data = "Web & UI/UX Designer";
        IV4FieldSerializer<string> serializer = new TitleFieldSerializer();
        var result = serializer.Write(data);

        result.ShouldNotBeNull();
        result.ShouldBe("TITLE:Web & UI/UX Designer");
    }
}