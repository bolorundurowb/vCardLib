using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class UrlFieldDeserializerTests
{
    [Test]
    public void Read_Should_DeserializeV2()
    {
        IV2FieldDeserializer<Url> deserializer = new UrlFieldDeserializer();
        var result = deserializer.Read("URL;WORK:www.foo.com");
        result.ShouldBe(new Url
        {
            Type = UrlType.Work,
            Value = "www.foo.com"
        });
    }

    [Test]
    public void Read_Should_DeserializeV3()
    {
        IV3FieldDeserializer<Url> deserializer = new UrlFieldDeserializer();
        var result = deserializer.Read("URL;TYPE=PROFILE;PREF=1;LABEL=\"LinkedIn Profile\":https://www.linkedin.com/in/john-doe");
        result.ShouldBe(new Url
        {
            Type = UrlType.Profile,
            Preference = 1,
            Label = "LinkedIn Profile",
            Value = "https://www.linkedin.com/in/john-doe"
        });
    }

    [Test]
    public void Read_Should_DeserializeV4()
    {
        IV4FieldDeserializer<Url> deserializer = new UrlFieldDeserializer();
        var result =
            deserializer.Read(
                "URL;TYPE=home;TYPE=blog;PREF=2;LABEL=My Home Page;MEDIA-TYPE=text/html;LANGUAGE=en;CHARSET=UTF-8:example.org");
        result.ShouldBe(new Url
        {
            Type = UrlType.Home | UrlType.Blog,
            Preference = 2,
            Label = "My Home Page",
            MimeType = "text/html",
            Language = "en",
            Charset = "UTF-8",
            Value = "example.org"
        });
    }
}