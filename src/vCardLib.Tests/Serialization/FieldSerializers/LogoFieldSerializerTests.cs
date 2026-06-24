using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class LogoFieldSerializerTests
{
    [Test]
    public void FieldKey_ReturnsLogo()
    {
        var serializer = new LogoFieldSerializer();
        serializer.FieldKey.ShouldBe("LOGO");
    }

    [Test]
    public void Write_V2SimpleLogo_ReturnsCorrectString()
    {
        var logo = new Photo("http://example.com/logo.png");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO:http://example.com/logo.png");
    }

    [Test]
    public void Write_V2WithEncoding_ReturnsCorrectString()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", "PNG");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;PNG;ENCODING=BASE64:SGVsbG8=");
    }

    [Test]
    public void Write_V3SimpleLogo_ReturnsCorrectString()
    {
        var logo = new Photo("http://example.com/logo.png", null, null, null, "http://example.com/logo.png");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;VALUE=http://example.com/logo.png:http://example.com/logo.png");
    }

    [Test]
    public void Write_V3WithEncoding_ReturnsCorrectString()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;VALUE=SGVsbG8=;ENCODING=b:SGVsbG8=");
    }

    [Test]
    public void Write_V4WithMimeType_ReturnsCorrectString()
    {
        var logo = new Photo("SGVsbG8=", "base64", null, "image/png", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;VALUE=SGVsbG8=;ENCODING=base64;MEDIATYPE=image/png:SGVsbG8=");
    }

    [Test]
    public void Write_V2_UsesDataProperty()
    {
        var logo = new Photo("actual-data", "BASE64", null, null, "debug-value");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldContain("actual-data");
        result.ShouldNotContain("debug-value");
    }

    [Test]
    public void Write_V3_UsesValueProperty_NotData()
    {
        var logo = new Photo("actual-base64-data", "BASE64", null, null, "debug-text");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;VALUE=debug-text;ENCODING=b:debug-text");
        result.ShouldNotContain("actual-base64-data");
    }

    [Test]
    public void Write_V4_UsesValueProperty_NotData()
    {
        var logo = new Photo("actual-base64-data", "base64", null, "image/png", "debug-text");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.ShouldBe("LOGO;VALUE=debug-text;ENCODING=base64;MEDIATYPE=image/png:debug-text");
        result.ShouldNotContain("actual-base64-data");
    }

    [Test]
    public void Write_V2_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("http://example.com/logo.png");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(logo.Data);
        roundTrip.Type.ShouldBe(logo.Type);
        roundTrip.Encoding.ShouldBe(logo.Encoding);
    }

    [Test]
    public void Write_V2WithEncoding_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", "PNG");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(logo.Data);
        roundTrip.Encoding.ShouldBe(logo.Encoding);
        roundTrip.Type.ShouldBe(logo.Type);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV3FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(logo.Data);
        roundTrip.Encoding.ShouldBe("BASE64");
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "base64", null, "image/png", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV4FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(logo.Data);
        roundTrip.MimeType.ShouldBe(logo.MimeType);
    }
}
