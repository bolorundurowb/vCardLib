using NUnit.Framework;
using OmniAssert;
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
        serializer.FieldKey.Must().Be("LOGO");
    }

    [Test]
    public void Write_V2_SimpleLogo_ReturnsExpectedWireFormat()
    {
        var logo = new Photo("http://example.com/logo.png");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO:http://example.com/logo.png");
    }

    [Test]
    public void Write_V2_WithEncoding_ReturnsExpectedWireFormat()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", "PNG");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;PNG;ENCODING=BASE64:SGVsbG8=");
    }

    [Test]
    public void Write_V3_SimpleLogo_ReturnsExpectedWireFormat()
    {
        var logo = new Photo("http://example.com/logo.png", null, null, null, "http://example.com/logo.png");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;VALUE=http://example.com/logo.png:http://example.com/logo.png");
    }

    [Test]
    public void Write_V3_WithEncoding_ReturnsExpectedWireFormat()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;VALUE=SGVsbG8=;ENCODING=b:SGVsbG8=");
    }

    [Test]
    public void Write_V4_WithMimeType_ReturnsExpectedWireFormat()
    {
        var logo = new Photo("SGVsbG8=", "base64", null, "image/png", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;VALUE=SGVsbG8=;ENCODING=base64;MEDIATYPE=image/png:SGVsbG8=");
    }

    [Test]
    public void Write_V2_UsesDataProperty()
    {
        var logo = new Photo("actual-data", "BASE64", null, null, "debug-value");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Contain("actual-data");
        result.Must().NotContain("debug-value");
    }

    [Test]
    public void Write_V3_UsesValueProperty_NotData()
    {
        var logo = new Photo("actual-base64-data", "BASE64", null, null, "debug-text");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;VALUE=debug-text;ENCODING=b:debug-text");
        result.Must().NotContain("actual-base64-data");
    }

    [Test]
    public void Write_V4_UsesValueProperty_NotData()
    {
        var logo = new Photo("actual-base64-data", "base64", null, "image/png", "debug-text");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        var result = serializer.Write(logo);

        result.Must().Be("LOGO;VALUE=debug-text;ENCODING=base64;MEDIATYPE=image/png:debug-text");
        result.Must().NotContain("actual-base64-data");
    }

    [Test]
    public void Write_V2_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("http://example.com/logo.png");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.Must().Be(logo.Data);
        roundTrip.Type.Must().Be(logo.Type);
        roundTrip.Encoding.Must().Be(logo.Encoding);
    }

    [Test]
    public void Write_V2_WithEncoding_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", "PNG");
        IV2FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.Must().Be(logo.Data);
        roundTrip.Encoding.Must().Be(logo.Encoding);
        roundTrip.Type.Must().Be(logo.Type);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV3FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.Must().Be(logo.Data);
        roundTrip.Encoding.Must().Be("BASE64");
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var logo = new Photo("SGVsbG8=", "base64", null, "image/png", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new LogoFieldSerializer();
        IV4FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(logo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.Must().Be(logo.Data);
        roundTrip.MimeType.Must().Be(logo.MimeType);
    }
}
