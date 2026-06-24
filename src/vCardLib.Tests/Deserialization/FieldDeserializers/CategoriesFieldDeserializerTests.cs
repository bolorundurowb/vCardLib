using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class CategoriesFieldDeserializerTests
{
    [Test]
    public void Read_UnexpectedInput_ReturnsEmptyList()
    {
        var input = string.Empty;
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeEmpty();
    }

    [Test]
    public void Read_SingleInput_ReturnsPopulatedList()
    {
        const string input = "CATEGORIES:TRAVEL AGENT";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
        result.ShouldContain("TRAVEL AGENT");
    }

    [Test]
    public void Read_MultipleInput_ReturnsPopulatedList()
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

    [Test]
    public void Read_LowercaseCategories_PreservesCase()
    {
        const string input = "CATEGORIES:alpha,beta,gamma";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Count.ShouldBe(3);
        result.ShouldContain("alpha");
        result.ShouldContain("beta");
        result.ShouldContain("gamma");
    }

    [Test]
    public void Read_ValueContainingCategoriesSubstring_ReturnsCorrectValue()
    {
        const string input = "CATEGORIES:CATEGORIES,test";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Count.ShouldBe(2);
        result.ShouldContain("CATEGORIES");
        result.ShouldContain("test");
    }
}