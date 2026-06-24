using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class PhotoFieldSerializerTests
{
    [Test]
    public void FieldKey_ReturnsPhoto()
    {
        var serializer = new PhotoFieldSerializer();
        serializer.FieldKey.ShouldBe("PHOTO");
    }

    [Test]
    public void Write_V2SimplePhoto_ReturnsCorrectString()
    {
        var photo = new Photo("http://example.com/photo.jpg");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO:http://example.com/photo.jpg");
    }

    [Test]
    public void Write_V2WithEncoding_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "BASE64");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;ENCODING=BASE64:SGVsbG8=");
    }

    [Test]
    public void Write_V2WithTypeAndEncoding_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "BASE64", "PNG");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;PNG;ENCODING=BASE64:SGVsbG8=");
    }

    [Test]
    public void Write_V3SimplePhoto_ReturnsCorrectString()
    {
        var photo = new Photo("http://example.com/photo.jpg", null, null, null, "uri");
        IV3FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;VALUE=uri:http://example.com/photo.jpg");
    }

    [Test]
    public void Write_V3WithEncoding_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;VALUE=SGVsbG8=;ENCODING=b:SGVsbG8=");
    }

    [Test]
    public void Write_V4WithMimeType_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "base64", null, "image/jpeg", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;VALUE=SGVsbG8=;ENCODING=base64;MEDIATYPE=image/jpeg:SGVsbG8=");
    }

    [Test]
    public void Write_V2_UsesDataProperty()
    {
        var photo = new Photo("actual-data", "BASE64", null, null, "debug-value");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldContain("actual-data");
        result.ShouldNotContain("debug-value");
    }

    [Test]
    public void Write_V3_UsesDataProperty_AfterColon()
    {
        var photo = new Photo("actual-base64-data", "BASE64", null, null, "debug-text");
        IV3FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;VALUE=debug-text;ENCODING=b:actual-base64-data");
        result.ShouldContain("actual-base64-data");
    }

    [Test]
    public void Write_V4_UsesDataProperty_AfterColon()
    {
        var photo = new Photo("actual-base64-data", "base64", null, "image/jpeg", "debug-text");
        IV4FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        var result = serializer.Write(photo);

        result.ShouldBe("PHOTO;VALUE=debug-text;ENCODING=base64;MEDIATYPE=image/jpeg:actual-base64-data");
        result.ShouldContain("actual-base64-data");
    }

    [Test]
    public void Write_V2_RoundTripsThroughDeserializer()
    {
        var photo = new Photo("http://example.com/photo.jpg");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(photo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(photo.Data);
        roundTrip.Type.ShouldBe(photo.Type);
        roundTrip.Encoding.ShouldBe(photo.Encoding);
    }

    [Test]
    public void Write_V2WithEncoding_RoundTripsThroughDeserializer()
    {
        var photo = new Photo("SGVsbG8=", "BASE64", "GIF");
        IV2FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        IV2FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(photo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(photo.Data);
        roundTrip.Encoding.ShouldBe(photo.Encoding);
        roundTrip.Type.ShouldBe(photo.Type);
    }

    [Test]
    public void Write_V3_RoundTripsThroughDeserializer()
    {
        var photo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        IV3FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        IV3FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(photo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(photo.Data);
        roundTrip.Encoding.ShouldBe("BASE64");
    }

    [Test]
    public void Write_V4_RoundTripsThroughDeserializer()
    {
        var photo = new Photo("SGVsbG8=", "base64", null, "image/jpeg", "SGVsbG8=");
        IV4FieldSerializer<Photo> serializer = new PhotoFieldSerializer();
        IV4FieldDeserializer<Photo> deserializer = new PhotoFieldDeserializer();

        var wire = serializer.Write(photo)!;
        var roundTrip = deserializer.Read(wire);

        roundTrip.Data.ShouldBe(photo.Data);
        roundTrip.MimeType.ShouldBe(photo.MimeType);
    }
}
