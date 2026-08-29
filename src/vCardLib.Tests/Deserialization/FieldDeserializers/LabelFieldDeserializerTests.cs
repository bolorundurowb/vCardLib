using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class LabelFieldDeserializerTests
{
    [Test]
    public void Read_V4_ReturnsNull()
    {
        const string input =
            @"LABEL;TYPE=dom,home,postal,parcel:Mr.John Q. Public\, Esq.\nMail Drop: TNE QB\n123 Main Street\nAny Town\, CA  91921-1234\nU.S.A.";
        IV4FieldDeserializer<Label?> deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.Must().BeNull();
    }

    [Test]
    public void Read_MultipleTypes_ReturnsExpectedValue()
    {
        const string input =
            @"LABEL;TYPE=dom,home,postal,parcel:Mr.John Q. Public\, Esq.\nMail Drop: TNE QB\n123 Main Street\nAny Town\, CA  91921-1234\nU.S.A.";
        var deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.Text.Must().Be(
            "Mr.John Q. Public, Esq." + Environment.NewLine +
            "Mail Drop: TNE QB" + Environment.NewLine +
            "123 Main Street" + Environment.NewLine +
            "Any Town, CA  91921-1234" + Environment.NewLine +
            "U.S.A.");
        result.Type.Must().Be(AddressType.Domestic | AddressType.Home | AddressType.Postal | AddressType.Parcel);
    }

    [Test]
    public void Read_SingleType_ReturnsExpectedValue()
    {
        const string input = @"LABEL;TYPE=HOME:123 Main St.\nSpringfield, IL 12345\nUSA";
        var deserializer = new LabelFieldDeserializer();
        var result = deserializer.Read(input);

        result.Text.Must().Be(
            "123 Main St." + Environment.NewLine +
            "Springfield, IL 12345" + Environment.NewLine +
            "USA");
        result.Type.Must().Be(AddressType.Home);
    }
}
