using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class FormattedNameDeserializerTests
{
    [Test]
    public void Read_ValidInput_ReturnsParsedValue()
    {
        const string input = @"FN:Mr. John Q. Public\, Esq.";
        var deserializer = new FormattedNameDeserializer();
        var result = deserializer.Read(input);

        result.Must().NotBeNull();
        result.Must().Be(@"Mr. John Q. Public\, Esq.");
    }
}