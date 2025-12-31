using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class NameFieldDeserializerTests
{
    [Test]
    public void Read_SimpleName_ReturnsCorrectName()
    {
        var input = "N:Doe;John;;;";
        var deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.ShouldBe("Doe");
        result.GivenName.ShouldBe("John");
        result.AdditionalNames.ShouldBe("");
        result.HonorificPrefix.ShouldBe("");
    }

    [Test]
    public void Read_FullName_ReturnsCorrectName()
    {
        var input = "N:Doe;John;Middle;Mr.;Esq.";
        var deserializer = new NameFieldDeserializer();
        var result = deserializer.Read(input);

        result.FamilyName.ShouldBe("Doe");
        result.GivenName.ShouldBe("John");
        result.AdditionalNames.ShouldBe("Middle");
        result.HonorificPrefix.ShouldBe("Mr.");
        result.HonorificSuffix.ShouldBe("Esq.");
    }
}
