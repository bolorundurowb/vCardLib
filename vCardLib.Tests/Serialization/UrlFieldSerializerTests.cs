using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization;

[TestFixture]
public class UrlFieldSerializerTests
{
    private Url data;

    [SetUp]
    public void SetUp()
    {
        data = new Url
        {
            Type = UrlType.Home | UrlType.Blog,
            Value = "example.org",
            Preference = 2,
            Label = "My Home Page",
            MimeType = "text/html",
            Language = "en",
            Charset = "UTF-8"
        };
    }

    [Test]
    public void Write_Should_SerializeV2()
    {
        IV2FieldSerializer<Url> serializer = new UrlFieldSerializer();
        var result = serializer.Write(data);
        result.ShouldBe(
            "URL;HOME;BLOG;PREF=2;LABEL=My Home Page;MEDIA-TYPE=text/html;LANGUAGE=en;CHARSET=UTF-8:example.org");
    }

    [Test]
    public void Write_Should_SerializeV3()
    {
        IV3FieldSerializer<Url> serializer = new UrlFieldSerializer();
        var result = serializer.Write(data);
        result.ShouldBe(
            "URL;TYPE=home;TYPE=blog;PREF=2;LABEL=My Home Page;MEDIA-TYPE=text/html;LANGUAGE=en;CHARSET=UTF-8:example.org");
    }

    [Test]
    public void Write_Should_SerializeV4()
    {
        IV4FieldSerializer<Url> serializer = new UrlFieldSerializer();
        var result = serializer.Write(data);
        result.ShouldBe(
            "URL;TYPE=home;TYPE=blog;PREF=2;LABEL=My Home Page;MEDIA-TYPE=text/html;LANGUAGE=en;CHARSET=UTF-8:example.org");
    }
}