using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Serialization.FieldSerializers;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class BirthdayFieldSerializerTests
{
    [Test]
    public void Write_ValidDate_ReturnsCorrectString()
    {
        var date = new DateTime(1990, 1, 1);
        var serializer = new BirthdayFieldSerializer();
        var result = serializer.Write(date);

        result.ShouldBe("BDAY:19900101");
    }
}
