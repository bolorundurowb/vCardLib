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

    [Test]
    public void Read_QuotedPrintable_DecodesValue()
    {
        const string input = "FN;CHARSET=UTF-8;ENCODING=QUOTED-PRINTABLE:=D0=92=D0=B0=D0=B4=D0=B8=D0=BC";
        var deserializer = new FormattedNameDeserializer();
        var result = deserializer.Read(input);

        result.Must().Be("Вадим");
    }
}