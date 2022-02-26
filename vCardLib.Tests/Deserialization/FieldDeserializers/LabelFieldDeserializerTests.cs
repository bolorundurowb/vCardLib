using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class LabelFieldDeserializerTests
{
    [Test]
    public void ShouldReturnNullForV4()
    {
        const string input = @"LABEL;TYPE=dom,home,postal,parcel:Mr.John Q. Public\, Esq.\nMail Drop: TNE QB\n123 Main Street\nAny Town\, CA  91921-1234\nU.S.A.";
        IV4FieldDeserializer<Label?> deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.ShouldBeNull();
    }
    
    [Test]
    public void ShouldReturnObjectWithMultipleTypes()
    {
        const string input = @"LABEL;TYPE=dom,home,postal,parcel:Mr.John Q. Public\, Esq.\nMail Drop: TNE QB\n123 Main Street\nAny Town\, CA  91921-1234\nU.S.A.";
        var deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.Text.ShouldBe("Mr.John Q. Public, Esq.\nMail Drop: TNE QB\n123 Main Street\nAny Town, CA  91921-1234\nU.S.A.");
        result.Types.Count.ShouldBe(4);
        result.Types.ShouldContain(AddressType.Domestic);
        result.Types.ShouldContain(AddressType.Home);
        result.Types.ShouldContain(AddressType.Postal);
        result.Types.ShouldContain(AddressType.Parcel);
    }
    
    [Test]
    public void ShouldReturnObjectWithSingleTypes()
    {
        const string input = @"LABEL;TYPE=HOME:123 Main St.\nSpringfield, IL 12345\nUSA";
        var deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.Text.ShouldBe("123 Main St.\nSpringfield, IL 12345\nUSA");
        result.Types.Count.ShouldBe(1);
        result.Types.ShouldContain(AddressType.Home);
    }
}