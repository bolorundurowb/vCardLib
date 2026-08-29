using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;

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
}
