using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class FormattedNameDeserializerTests
{
    [Test]
    public void ShouldReturnParsedValue()
    {
        const string input = @"FN:Mr. John Q. Public\, Esq.";
        var deserializer = new FormattedNameDeserializer();
        var result = deserializer.Read(input);

        result.ShouldNotBeNull();
        result.ShouldBe(@"Mr. John Q. Public\, Esq.");
    }
}