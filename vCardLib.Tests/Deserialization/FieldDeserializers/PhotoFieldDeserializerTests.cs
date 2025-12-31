using NUnit.Framework;
using Shouldly;
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
    public void Read_V2_SimpleUrl_ReturnsPhoto()
    {
        var input = "PHOTO:http://www.abc.com/pub/photos/jqpublic.gif";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("http://www.abc.com/pub/photos/jqpublic.gif");
        // In V2 simple case with no params, these should be null
        result.Type.ShouldBeNull();
        result.Encoding.ShouldBeNull();
    }

    [Test]
    public void Read_V2_WithEncodingAndImplicitType_ReturnsPopulatedPhoto()
    {
        var input = "PHOTO;GIF;ENCODING=BASE64:R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
        result.Encoding.ShouldBe("BASE64");
        result.Type.ShouldBe("GIF"); 
    }

    [Test]
    public void Read_V2_WithExplicitTypeParam_SetsKeyAsType()
    {
        var input = "PHOTO;TYPE=JPEG:http://example.com/photo.jpg";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Type.ShouldBe("TYPE");
    }

    [Test]
    public void Read_V2_DataUri_ParsesDataUri()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";
        var result = ((IV2FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe(rawData);
        result.MimeType.ShouldBe("image/jpeg");
        result.Encoding.ShouldBe("base64");
    }

    #endregion

    #region V3 Tests

    [Test]
    public void Read_V3_WithBinaryEncoding_NormalizesToBase64()
    {
        var input = "PHOTO;ENCODING=b;TYPE=JPEG:MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA");
        result.Encoding.ShouldBe("BASE64");
        result.Type.ShouldBe("JPEG");
    }

    [Test]
    public void Read_V3_WithUriValue_ReturnsPhoto()
    {
        var input = "PHOTO;VALUE=uri;TYPE=GIF:http://www.abc.com/pub/photos/jqpublic.gif";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("http://www.abc.com/pub/photos/jqpublic.gif");
        result.Value.ShouldBe("uri");
        result.Type.ShouldBe("GIF");
    }

    [Test]
    public void Read_V3_DataUri_ParsesDataUri()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";
        var result = ((IV3FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe(rawData);
        result.MimeType.ShouldBe("image/jpeg");
        result.Encoding.ShouldBe("base64");
    }

    #endregion

    #region V4 Tests

    [Test]
    public void Read_V4_StandardUri_ReturnsPhoto()
    {
        var input = "PHOTO:http://www.example.com/pub/photos/jqpublic.gif";
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("http://www.example.com/pub/photos/jqpublic.gif");
        result.MimeType.ShouldBeNull(); 
    }

    [Test]
    public void Read_V4_DataUri_ParsesMimeTypeAndEncoding()
    {
        var rawData = "MIICajCCAdOgAwIBAgICBEUwDQYJKoZIhvcNAQEEBQA";
        var input = $"PHOTO:data:image/jpeg;base64,{rawData}";
        
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe(rawData);
        result.MimeType.ShouldBe("image/jpeg");
        result.Encoding.ShouldBe("base64");
    }

    [Test]
    public void Read_V4_WithMediaTypeParameter_ReturnsPhoto()
    {
        var input = "PHOTO;MEDIATYPE=image/jpeg:http://example.com/photo.jpg";
        
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);

        result.Data.ShouldBe("http://example.com/photo.jpg");
        result.MimeType.ShouldBe("image/jpeg");
    }
    
    [Test]
    public void Read_V4_DataUri_StripsPrefixCorrectly()
    {
        var input = "PHOTO:data:image/png;base64,ABC12345";
        var result = ((IV4FieldDeserializer<Photo>)_deserializer).Read(input);
        
        result.Data.ShouldBe("ABC12345");
        result.MimeType.ShouldBe("image/png");
    }

    #endregion
}
