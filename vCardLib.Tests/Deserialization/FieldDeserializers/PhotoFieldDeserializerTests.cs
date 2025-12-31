using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class PhotoFieldDeserializerTests
{
    [Test]
    public void Read_V2_SimplePhoto_ReturnsPhoto()
    {
        var input = "PHOTO:http://example.com/photo.jpg";
        var deserializer = new PhotoFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<Photo>).Read(input);

        result.Data.ShouldBe("http://example.com/photo.jpg");
    }

    [Test]
    public void Read_V2_WithEncoding_ReturnsPhoto()
    {
        var input = "PHOTO;ENCODING=BASE64:SGVsbG8=";
        var deserializer = new PhotoFieldDeserializer();
        var result = (deserializer as IV2FieldDeserializer<Photo>).Read(input);

        result.Data.ShouldBe("SGVsbG8=");
        result.Encoding.ShouldBe("BASE64");
    }

    [Test]
    public void Read_V3_WithEncoding_ReturnsPhoto()
    {
        var input = "PHOTO;ENCODING=b:SGVsbG8=";
        var deserializer = new PhotoFieldDeserializer();
        var result = (deserializer as IV3FieldDeserializer<Photo>).Read(input);

        result.Data.ShouldBe("SGVsbG8=");
        result.Encoding.ShouldBe("BASE64");
    }

    [Test]
    public void Read_V4_DataUri_ReturnsPhoto()
    {
        var input = "PHOTO:data:image/jpeg;base64,SGVsbG8=";
        var deserializer = new PhotoFieldDeserializer();
        var result = (deserializer as IV4FieldDeserializer<Photo>).Read(input);

        result.Data.ShouldBe("SGVsbG8=");
        result.MimeType.ShouldBe("image/jpeg");
        result.Encoding.ShouldBe("base64");
    }
}
