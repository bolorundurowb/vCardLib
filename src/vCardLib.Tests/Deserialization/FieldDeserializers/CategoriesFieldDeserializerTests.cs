using NUnit.Framework;
using OmniAssert;
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

        result.Must().BeEmpty();
    }

    [Test]
    public void Read_SingleInput_ReturnsPopulatedList()
    {
        const string input = "CATEGORIES:TRAVEL AGENT";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeEmpty();
        result.Count.Must().Be(1);
        result.Must().Contain("TRAVEL AGENT");
    }

    [Test]
    public void Read_MultipleInput_ReturnsPopulatedList()
    {
        const string input = "CATEGORIES:INTERNET,IETF,INDUSTRY,INFORMATION TECHNOLOGY";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeEmpty();
        result.Count.Must().Be(4);
        result.Must().Contain("INTERNET");
        result.Must().Contain("IETF");
        result.Must().Contain("INDUSTRY");
        result.Must().Contain("INFORMATION TECHNOLOGY");
    }

    [Test]
    public void Read_LowercaseCategories_PreservesCase()
    {
        const string input = "CATEGORIES:alpha,beta,gamma";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Count.Must().Be(3);
        result.Must().Contain("alpha");
        result.Must().Contain("beta");
        result.Must().Contain("gamma");
    }

    [Test]
    public void Read_ValueContainingCategoriesSubstring_ReturnsExpectedValue()
    {
        const string input = "CATEGORIES:CATEGORIES,test";
        var deserializer = new CategoriesFieldDeserializer();
        var result = deserializer.Read(input);

        result.Count.Must().Be(2);
        result.Must().Contain("CATEGORIES");
        result.Must().Contain("test");
    }
}