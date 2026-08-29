using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class PhotoFieldDeserializerTests
{
    private PhotoFieldDeserializer _deserializer;

    [SetUp]
    public void Setup()
    {
        _deserializer = new PhotoFieldDeserializer();
    }

    #region V2 Tests

    [Test]
    public void Read_V2_SimpleUrl_ReturnsExpectedValue()
    {
        var input = "PHOTO:http://www.abc.com/pub/photos/jqpublic.gif";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("http://www.abc.com/pub/photos/jqpublic.gif");
        // In V2 simple case with no params, these should be null
        result.Type.Must().BeNull();
        result.Encoding.Must().BeNull();
    }

    [Test]
    public void Read_V2_WithEncodingAndImplicitType_ReturnsExpectedValue()
    {
        var input = "PHOTO;GIF;ENCODING=BASE64:R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
        result.Encoding.Must().Be("BASE64");
        result.Type.Must().Be("GIF");
    }

    [Test]
    public void Read_V2_WithExplicitTypeParam_ReturnsExpectedValue()
    {
        var input = "PHOTO;TYPE=JPEG:http://example.com/photo.jpg";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Type.Must().Be("TYPE");
    }

    [Test]
    public void Read_V2_DataUri_ReturnsExpectedValue()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be(rawData);
        result.MimeType.Must().Be("image/jpeg");
        result.Encoding.Must().Be("base64");
    }

    #endregion

    #region V3 Tests

    [Test]
    public void Read_V3_WithBinaryEncoding_NormalizesToBase64()
    {
        var input = "PHOTO;ENCODING=b;TYPE=JPEG:MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA");
        result.Encoding.Must().Be("BASE64");
        result.Type.Must().Be("JPEG");
    }

    [Test]
    public void Read_V3_WithUriValue_ReturnsExpectedValue()
    {
        var input = "PHOTO;VALUE=uri;TYPE=GIF:http://www.abc.com/pub/photos/jqpublic.gif";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("http://www.abc.com/pub/photos/jqpublic.gif");
        result.Value.Must().Be("uri");
        result.Type.Must().Be("GIF");
    }

    [Test]
    public void Read_V3_DataUri_ReturnsExpectedValue()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be(rawData);
        result.MimeType.Must().Be("image/jpeg");
        result.Encoding.Must().Be("base64");
    }

    #endregion

    #region V4 Tests

    [Test]
    public void Read_V4_StandardUri_ReturnsExpectedValue()
    {
        var input = "PHOTO:http://www.example.com/pub/photos/jqpublic.gif";
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("http://www.example.com/pub/photos/jqpublic.gif");
        result.MimeType.Must().BeNull();
    }

    [Test]
    public void Read_V4_DataUri_ReturnsExpectedValue()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";

        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be(rawData);
        result.MimeType.Must().Be("image/jpeg");
        result.Encoding.Must().Be("base64");
    }

    [Test]
    public void Read_V4_WithMediaTypeParameter_ReturnsExpectedValue()
    {
        var input = "PHOTO;MEDIATYPE=image/jpeg:http://example.com/photo.jpg";

        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("http://example.com/photo.jpg");
        result.MimeType.Must().Be("image/jpeg");
    }

    [Test]
    public void Read_V4_DataUri_StripsPrefixCorrectly()
    {
        var input = "PHOTO:data:image/png;base64,ABC12345";
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.Must().Be("ABC12345");
        result.MimeType.Must().Be("image/png");
    }

    #endregion
}
