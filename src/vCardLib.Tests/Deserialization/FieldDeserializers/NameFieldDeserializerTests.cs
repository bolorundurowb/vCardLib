using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NameFieldDeserializerTests
{
    [Test]
    public void Read_SimpleName_ReturnsExpectedName()
    {
        var input = "N:Doe;John;;;";
        var deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.Must().Be("Doe");
        result.GivenName.Must().Be("John");
        result.AdditionalNames.Must().Be("");
        result.HonorificPrefix.Must().Be("");
    }

    [Test]
    public void Read_FullName_ReturnsExpectedName()
    {
        var input = "N:Doe;John;Middle;Mr.;Esq.";
        var deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.Must().Be("Doe");
        result.GivenName.Must().Be("John");
        result.AdditionalNames.Must().Be("Middle");
        result.HonorificPrefix.Must().Be("Mr.");
        result.HonorificSuffix.Must().Be("Esq.");
    }

    [Test]
    public void Read_QuotedPrintable_DecodesValue()
    {
        var input = "N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=D0=BC;;;";
        var deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.Must().Be("Вадим");
        result.GivenName.Must().Be("");
    }

    [Test]
    public void Read_V2_QuotedPrintable_DecodesValue()
    {
        var input = "N;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=D0=BC;;;";
        IV2FieldDeserializer<Name> deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.Must().Be("Вадим");
        result.GivenName.Must().Be("");
    }
}
