using NUnit.Framework;
using Shouldly;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class KeyFieldSerializerTests
{
    [Test]
    public void V2_Write_IncludesTypeAndValue()
    {
        IV2FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("http://example.com/key.asc", type: "PGP", encoding: "BASE64");

        var line = serializer.Write(key)!;

        line.ShouldStartWith("KEY;");
        line.ShouldContain("pgp");
        line.ShouldContain("ENCODING=BASE64");
        line.ShouldContain("http://example.com/key.asc");
    }

    [Test]
    public void V2_Write_ValueOnly_OmitsOptionalParameters()
    {
        IV2FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("plain-value");

        var line = serializer.Write(key)!;

        line.ShouldBe("KEY:plain-value");
    }

    [Test]
    public void V3_Write_Base64Encoding_UsesShortBParameter()
    {
        IV3FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("dGVzdA==", type: "PGP", encoding: "BASE64");

        var line = serializer.Write(key)!;

        line.ShouldContain("TYPE=PGP");
        line.ShouldContain("ENCODING=b");
        line.ShouldNotContain("ENCODING=BASE64");
        line.ShouldContain("dGVzdA==");
    }

    [Test]
    public void V3_Write_NonBase64Encoding_PreservedVerbatim()
    {
        IV3FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("x", type: null, encoding: "8BIT");

        var line = serializer.Write(key)!;

        line.ShouldContain("ENCODING=8BIT");
    }

    [Test]
    public void V4_Write_Base64Encoding_UsesInlineBase64Form()
    {
        IV4FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("QUJD", type: null, mimeType: null, encoding: "BASE64");

        var line = serializer.Write(key)!;

        line.ShouldContain("base64,QUJD");
        line.ShouldNotContain(":QUJD");
    }

    [Test]
    public void V4_Write_NonBase64_IncludesMediaTypeAndColonValue()
    {
        IV4FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("ftp://keys/jdoe", type: "work", mimeType: "application/pgp-keys", encoding: null);

        var line = serializer.Write(key)!;

        line.ShouldContain("TYPE=work");
        line.ShouldContain("MEDIATYPE=application/pgp-keys");
        line.ShouldContain(":ftp://keys/jdoe");
    }
}
