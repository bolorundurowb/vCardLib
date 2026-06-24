using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Tests.Serialization.FieldSerializers;

/// <summary>
/// Smoke tests for field serializers that do not yet have dedicated test fixtures.
/// </summary>
[TestFixture]
public class MiscellaneousFieldSerializerTests
{
    [Test]
    public void Write_V4_Anniversary_ReturnsExpectedWireFormat()
    {
        var date = new DateTime(2000, 1, 1);
        var serializer = new AnniversaryFieldSerializer();
        var result = ((IV4FieldSerializer<DateTime>)serializer).Write(date);
        result.ShouldBe("ANNIVERSARY:20000101");
    }

    [Test]
    public void Write_V4_Gender_ReturnsExpectedWireFormat()
    {
        var gender = new Gender(BiologicalSex.Male, "Man");
        var serializer = new GenderFieldSerializer();
        var result = ((IV4FieldSerializer<Gender>)serializer).Write(gender);
        result.ShouldBe("GENDER:M;Man");
    }

    [Test]
    public void Write_Organization_ReturnsExpectedWireFormat()
    {
        var org = new Organization("Company", "IT", "Dev");
        var serializer = new OrganizationFieldSerializer();
        var result = serializer.Write(org);
        result.ShouldBe("ORG:Company;IT;Dev");
    }

    [Test]
    public void Write_V2_FormattedName_ReturnsExpectedWireFormat()
    {
        var serializer = new FormattedNameSerializer();
        var result = ((IV2FieldSerializer<string>)serializer).Write("John Doe");
        result.ShouldBe("FN:John Doe");
    }

    [Test]
    public void Write_Categories_ReturnsExpectedWireFormat()
    {
        var serializer = new CategoriesFieldSerializer();
        var data = new List<string> { "Work", "Friend" };
        var result = serializer.Write(data);
        result.ShouldBe("CATEGORIES:Work,Friend");
    }

    [Test]
    public void Write_Agent_ReturnsExpectedWireFormat()
    {
        var serializer = new AgentFieldSerializer();
        var result = serializer.Write("http://example.com/agent");
        result.ShouldBe("AGENT;VALUE=uri:http://example.com/agent");
    }

    [Test]
    public void Write_Revision_ReturnsExpectedWireFormat()
    {
        var serializer = new RevisionFieldSerializer();
        var date = new DateTime(2023, 10, 27, 10, 0, 0, DateTimeKind.Utc);
        var result = serializer.Write(date);
        result.ShouldBe("REV:20231027T100000Z");
    }

    [Test]
    public void Write_Uid_ReturnsExpectedWireFormat()
    {
        var serializer = new UidFieldSerializer();
        var result = serializer.Write("12345");
        result.ShouldBe("UID:12345");
    }

    [Test]
    public void Write_V2_Mailer_ReturnsExpectedWireFormat()
    {
        var serializer = new MailerFieldSerializer();
        var result = serializer.Write("PicoMail");
        result.ShouldBe("MAILER:PicoMail");
    }

    [Test]
    public void Write_CustomField_ReturnsExpectedWireFormat()
    {
        var serializer = new CustomFieldSerializer();
        var result = serializer.Write(new KeyValuePair<string, string>("X-SOCIAL", "Twitter"));
        result.ShouldBe("X-SOCIAL:Twitter");
    }

    [Test]
    public void Write_Kind_ReturnsExpectedWireFormat()
    {
        var serializer = new KindSerializer();
        var result = serializer.Write(ContactKind.Individual);
        result.ShouldBe("KIND:individual");
    }
}
