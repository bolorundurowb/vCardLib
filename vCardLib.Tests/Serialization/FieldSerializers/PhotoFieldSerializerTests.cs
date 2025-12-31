using NUnit.Framework;
using Shouldly;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class PhotoFieldSerializerTests
{
    [Test]
    public void Write_V2_SimplePhoto_ReturnsCorrectString()
    {
        var photo = new Photo("http://example.com/photo.jpg");
        var serializer = new PhotoFieldSerializer();
        var result = (serializer as IV2FieldSerializer<Photo>).Write(photo);

        result.ShouldBe("PHOTO:http://example.com/photo.jpg");
    }

    [Test]
    public void Write_V2_WithEncoding_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "BASE64");
        var serializer = new PhotoFieldSerializer();
        var result = (serializer as IV2FieldSerializer<Photo>).Write(photo);

        result.ShouldBe("PHOTO;ENCODING=BASE64:SGVsbG8=");
    }

    [Test]
    public void Write_V3_WithEncoding_ReturnsCorrectString()
    {
        // For V3, the code uses data.Value instead of data.Data for the value part.
        var photo = new Photo("SGVsbG8=", "BASE64", null, null, "SGVsbG8=");
        var serializer = new PhotoFieldSerializer();
        var result = (serializer as IV3FieldSerializer<Photo>).Write(photo);

        result.ShouldBe("PHOTO;VALUE=SGVsbG8=;ENCODING=b:SGVsbG8=");
    }

    [Test]
    public void Write_V4_WithMimeType_ReturnsCorrectString()
    {
        var photo = new Photo("SGVsbG8=", "base64", null, "image/jpeg", "SGVsbG8=");
        var serializer = new PhotoFieldSerializer();
        var result = (serializer as IV4FieldSerializer<Photo>).Write(photo);

        result.ShouldBe("PHOTO;VALUE=SGVsbG8=;ENCODING=base64;MEDIATYPE=image/jpeg:SGVsbG8=");
    }
}
