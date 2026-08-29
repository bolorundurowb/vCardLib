using NUnit.Framework;
using OmniAssert;
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

        line.Must().StartWith("KEY;");
        line.Must().Contain("pgp");
        line.Must().Contain("ENCODING=base64");
        line.Must().Contain("http://example.com/key.asc");
    }

    [Test]
    public void V2_Write_ValueOnly_OmitsOptionalParameters()
    {
        IV2FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("plain-value");

        var line = serializer.Write(key)!;

        line.Must().Be("KEY:plain-value");
    }

    [Test]
    public void V3_Write_Base64Encoding_UsesShortBParameter()
    {
        IV3FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("dGVzdA==", type: "PGP", encoding: "BASE64");

        var line = serializer.Write(key)!;

        line.Must().Contain("TYPE=pgp");
        line.Must().Contain("ENCODING=b");
        line.Must().NotContain("ENCODING=base64");
        line.Must().Contain("dGVzdA==");
    }

    [Test]
    public void V3_Write_NonBase64Encoding_PreservedVerbatim()
    {
        IV3FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("x", type: null, encoding: "8BIT");

        var line = serializer.Write(key)!;

        line.Must().Contain("ENCODING=8bit");
    }

    [Test]
    public void V4_Write_Base64Encoding_UsesInlineBase64Form()
    {
        IV4FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("QUJD", type: null, mimeType: null, encoding: "BASE64");

        var line = serializer.Write(key)!;

        line.Must().Contain("base64,QUJD");
        line.Must().NotContain(":QUJD");
    }

    [Test]
    public void V4_Write_NonBase64_IncludesMediaTypeAndColonValue()
    {
        IV4FieldSerializer<Key> serializer = new KeyFieldSerializer();
        var key = new Key("ftp://keys/jdoe", type: "work", mimeType: "application/pgp-keys", encoding: null);

        var line = serializer.Write(key)!;

        line.Must().Contain("TYPE=work");
        line.Must().Contain("MEDIATYPE=application/pgp-keys");
        line.Must().Contain(":ftp://keys/jdoe");
    }
}
