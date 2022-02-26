using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class CategoriesFieldDeserializerTests
{
    [Test]
    public void ShouldReturnAnEmptyListWithUnexpectedInput()
    {
        var input = string.Empty;
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeEmpty();
    }

    [Test] 
    public void ShouldReturnPopulatedListWithSingleInput()
    {
        const string input = "CATEGORIES:TRAVEL AGENT";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result.ShouldContain("TRAVEL AGENT");
    }

    [Test] 
    public void ShouldReturnPopulatedListWithMultipleInput()
    {
        const string input = "CATEGORIES:INTERNET,IETF,INDUSTRY,INFORMATION TECHNOLOGY";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(4);
        result.ShouldContain("INTERNET");
        result.ShouldContain("IETF");
        result.ShouldContain("INDUSTRY");
        result.ShouldContain("INFORMATION TECHNOLOGY");
    }
}