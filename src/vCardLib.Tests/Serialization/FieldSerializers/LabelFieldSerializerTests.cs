using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class LabelFieldSerializerTests
{
    [Test]
    public void Write_NoneType_ReturnsLabelWithValueOnly()
    {
        IV2FieldSerializer<Label> serializer = new LabelFieldSerializer();
        var label = new Label("123 Main St", AddressType.None);

        var line = serializer.Write(label)!;

        line.ShouldBe("LABEL:123 Main St");
    }

    [Test]
    public void Write_SingleAddressType_AppendsTypeParameter()
    {
        IV2FieldSerializer<Label> serializer = new LabelFieldSerializer();
        var label = new Label("HQ", AddressType.Work);

        var line = serializer.Write(label)!;

        line.ShouldContain("TYPE=work");
        line.ShouldContain("HQ");
    }

    [Test]
    public void Write_MultipleAddressTypes_AppendsEachType()
    {
        IV2FieldSerializer<Label> serializer = new LabelFieldSerializer();
        var label = new Label("Home office", AddressType.Home | AddressType.Postal);

        var line = serializer.Write(label)!;

        line.ShouldContain("TYPE=home");
        line.ShouldContain("TYPE=postal");
    }

    [Test]
    public void V4_Write_ReturnsNull()
    {
        IV4FieldSerializer<Label> serializer = new LabelFieldSerializer();
        var label = new Label("ignored", AddressType.Home);

        serializer.Write(label).ShouldBeNull();
    }
}
