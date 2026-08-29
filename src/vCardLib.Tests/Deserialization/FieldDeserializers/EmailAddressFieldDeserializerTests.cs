using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Tests.Deserialization.FieldDeserializers;

[TestFixture]
public class EmailAddressFieldDeserializerTests
{
    [Test]
    public void Read_V2_SimpleInput_ReturnsExpectedValue()
    {
        const string input = "EMAIL:johndoe@hotmail.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().BeNull();
        result.Type.Must().Be(EmailAddressType.None);
        result.Value.Must().Be("johndoe@hotmail.com");
    }

    [Test]
    public void Read_V3_ComplexInput_ReturnsExpectedValue()
    {
        const string input = "EMAIL;type=INTERNET;type=WORK;pref:johnDoe@example.org";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().Be(1);
        result.Type.HasFlag(EmailAddressType.Internet).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Work).Must().BeTrue();
        result.Value.Must().Be("johnDoe@example.org");
    }

    [Test]
    public void Read_V4_ComplexInput_ReturnsExpectedValue()
    {
        const string input = "EMAIL;type=Aol;type=HOME;pref=1:johnDoe@example.org";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().Be(1);
        result.Type.HasFlag(EmailAddressType.Aol).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Home).Must().BeTrue();
        result.Value.Must().Be("johnDoe@example.org");
    }

    [Test]
    public void Read_V2_WithBarePref_ReturnsPreferenceOne()
    {
        const string input = "EMAIL;PREF:test@example.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().Be(1);
        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V3_WithBarePref_ReturnsPreferenceOne()
    {
        const string input = "EMAIL;PREF:test@example.com";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().Be(1);
        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V4_WithExplicitPrefValue_ReturnsExpectedPreference()
    {
        const string input = "EMAIL;PREF=2:test@example.com";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().Be(2);
        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V4_WithInvalidPrefValue_ReturnsNull()
    {
        const string input = "EMAIL;PREF=invalid:test@example.com";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Preference.Must().BeNull();
        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V2_WithQuotedPrintable_DecodesValue()
    {
        const string input = "EMAIL;ENCODING=QUOTED-PRINTABLE:test=40example.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V3_WithQuotedPrintable_DecodesValue()
    {
        const string input = "EMAIL;ENCODING=QUOTED-PRINTABLE:test=40example.com";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V4_WithQuotedPrintable_DecodesValue()
    {
        const string input = "EMAIL;ENCODING=QUOTED-PRINTABLE:test=40example.com";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V2_WithMultipleTypes_CombinesTypes()
    {
        const string input = "EMAIL;INTERNET;WORK;HOME:test@example.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(EmailAddressType.Internet).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Work).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Home).Must().BeTrue();
    }

    [Test]
    public void Read_V3_WithMultipleTypes_CombinesTypes()
    {
        const string input = "EMAIL;TYPE=INTERNET;TYPE=WORK;TYPE=HOME:test@example.com";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(EmailAddressType.Internet).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Work).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Home).Must().BeTrue();
    }

    [Test]
    public void Read_V4_WithMultipleTypes_CombinesTypes()
    {
        const string input = "EMAIL;TYPE=INTERNET;TYPE=WORK;TYPE=HOME:test@example.com";
        IV4FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.HasFlag(EmailAddressType.Internet).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Work).Must().BeTrue();
        result.Type.HasFlag(EmailAddressType.Home).Must().BeTrue();
    }

    [Test]
    public void Read_V2_WithUnknownType_IgnoresType()
    {
        const string input = "EMAIL;UNKNOWN:test@example.com";
        IV2FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.Must().Be(EmailAddressType.None);
        result.Value.Must().Be("test@example.com");
    }

    [Test]
    public void Read_V3_WithNoParameters_ReturnsSimpleEmail()
    {
        const string input = "EMAIL:test@example.com";
        IV3FieldDeserializer<EmailAddress> deserializer = new EmailAddressFieldDeserializer();
        var result = deserializer.Read(input);

        result.Type.Must().Be(EmailAddressType.None);
        result.Preference.Must().BeNull();
        result.Value.Must().Be("test@example.com");
    }
}
