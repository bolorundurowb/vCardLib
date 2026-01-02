using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

[TestFixture]
public class MiscellaneousFieldSerializerTests
{
    [Test]
    public void AnniversaryFieldSerializer_WriteV4_ReturnsCorrectString()
    {
        var date = new DateTime(2000, 1, 1);
        var serializer = new AnniversaryFieldSerializer();
        var result = ((IV4FieldSerializer<DateTime>)serializer).Write(date);
        result.ShouldBe("ANNIVERSARY:20000101");
    }

    [Test]
    public void BirthdayFieldSerializer_Write_ReturnsCorrectString()
    {
        var date = new DateTime(2000, 1, 1);
        var serializer = new BirthdayFieldSerializer();
        var result = serializer.Write(date);
        result.ShouldBe("BDAY:20000101");
    }

    [Test]
    public void GenderFieldSerializer_WriteV4_ReturnsCorrectString()
    {
        var gender = new Gender(BiologicalSex.Male, "Man");
        var serializer = new GenderFieldSerializer();
        var result = ((IV4FieldSerializer<Gender>)serializer).Write(gender);
        result.ShouldBe("GENDER:M;Man");
    }

    [Test]
    public void OrganizationFieldSerializer_Write_ReturnsCorrectString()
    {
        var org = new Organization("Company", "IT", "Dev");
        var serializer = new OrganizationFieldSerializer();
        var result = serializer.Write(org);
        result.ShouldBe("ORG:Company;IT;Dev");
    }

    [Test]
    public void FormattedNameSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new FormattedNameSerializer();
        var result = ((IV2FieldSerializer<string>)serializer).Write("John Doe");
        result.ShouldBe("FN:John Doe");
    }

    [Test]
    public void CategoriesFieldSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new CategoriesFieldSerializer();
        var data = new List<string> { "Work", "Friend" };
        var result = serializer.Write(data);
        result.ShouldBe("CATEGORIES: Work,Friend");
    }

    [Test]
    public void MemberFieldSerializer_WriteV4_ReturnsCorrectString()
    {
        var serializer = new MemberFieldSerializer();
        var result = ((IV4FieldSerializer<string>)serializer).Write("mailto:john@example.com");
        result.ShouldBe("MEMBER:mailto:john@example.com");
    }

    [Test]
    public void AgentFieldSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new AgentFieldSerializer();
        var result = serializer.Write("http://example.com/agent");
        result.ShouldBe("AGENT;VALUE=uri:http://example.com/agent");
    }

    [Test]
    public void RevisionFieldSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new RevisionFieldSerializer();
        var date = new DateTime(2023, 10, 27, 10, 0, 0, DateTimeKind.Utc);
        var result = serializer.Write(date);
        result.ShouldBe("REV:20231027T100000Z");
    }

    [Test]
    public void UidFieldSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new UidFieldSerializer();
        var result = serializer.Write("12345");
        result.ShouldBe("UID:12345");
    }

    [Test]
    public void VersionFieldSerializer_Write_ReturnsCorrectString()
    {
        var result = VersionFieldSerializer.Write(vCardVersion.v3);
        result.ShouldBe("VERSION:3.0");
    }

    [Test]
    public void MailerFieldSerializer_WriteV2V3_ReturnsCorrectString()
    {
        var serializer = new MailerFieldSerializer();
        var result = serializer.Write("PicoMail");
        result.ShouldBe("MAILER:PicoMail");
    }

    [Test]
    public void CustomFieldSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new CustomFieldSerializer();
        var result = serializer.Write(new KeyValuePair<string, string>("X-SOCIAL", "Twitter"));
        result.ShouldBe("X-SOCIAL: Twitter");
    }

    [Test]
    public void KindSerializer_Write_ReturnsCorrectString()
    {
        var serializer = new KindSerializer();
        var result = serializer.Write(ContactKind.Individual);
        result.ShouldBe("KIND:individual");
    }
}
