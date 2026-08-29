using System;
using NUnit.Framework;
using OmniAssert;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class SharedParsersTests
{
    [Test]
    public void ParseDate_YyyyMmDd_ReturnsUtcDate()
    {
        var result = SharedParsers.ParseDate("19900515");
        result.Must().NotBeNull();
        result!.Value.Year.Must().Be(1990);
        result.Value.Month.Must().Be(5);
        result.Value.Day.Must().Be(15);
    }

    [Test]
    public void ParseDate_YyyyMmDdThhMmSsZ_ReturnsUtc()
    {
        var result = SharedParsers.ParseDate("19900515T120000Z");
        result.Must().NotBeNull();
        result!.Value.Hour.Must().Be(12);
    }

    [Test]
    public void ParseDate_HhMmTime_ReturnsDateWithTimeOffset()
    {
        var result = SharedParsers.ParseDate("0630");
        result.Must().NotBeNull();
    }

    [Test]
    public void ParseDate_Unrecognized_ReturnsNull()
    {
        SharedParsers.ParseDate("not-a-date").Must().BeNull();
    }

    [Test]
    public void ParseDate_WithParameters_ReturnsDate()
    {
        var result = SharedParsers.ParseDate(";VALUE=DATE:19900515");
        result.Must().NotBeNull();
        result!.Value.Year.Must().Be(1990);
        result.Value.Month.Must().Be(5);
        result.Value.Day.Must().Be(15);
    }

    [Test]
    public void ParseDate_WithColonPrefix_ReturnsDate()
    {
        var result = SharedParsers.ParseDate(":19900515");
        result.Must().NotBeNull();
        result!.Value.Year.Must().Be(1990);
    }

    [TestCase("home", AddressType.Home)]
    [TestCase("HOME", AddressType.Home)]
    [TestCase("work", AddressType.Work)]
    [TestCase("dom", AddressType.Domestic)]
    [TestCase("intl", AddressType.International)]
    [TestCase("parcel", AddressType.Parcel)]
    [TestCase("postal", AddressType.Postal)]
    public void ParseAddressType_KnownKeys_ReturnsType(string key, AddressType expected)
    {
        key.ParseAddressType().Must().Be(expected);
    }

    [Test]
    public void ParseAddressType_Unknown_ReturnsNull()
    {
        "unknown-type".ParseAddressType().Must().BeNull();
    }

    [TestCase("internet", EmailAddressType.Internet)]
    [TestCase("pref", EmailAddressType.Preferred)]
    [TestCase("aol", EmailAddressType.Aol)]
    [TestCase("ibmmail", EmailAddressType.IbmMail)]
    [TestCase("applelink", EmailAddressType.Applelink)]
    public void ParseEmailAddressType_KnownKeys_ReturnsType(string key, EmailAddressType expected)
    {
        SharedParsers.ParseEmailAddressType(key).Must().Be(expected);
    }

    [Test]
    public void ParseEmailAddressType_Unknown_ReturnsNull()
    {
        SharedParsers.ParseEmailAddressType("satellite").Must().BeNull();
    }

    [TestCase("voice", TelephoneNumberType.Voice)]
    [TestCase("cell", TelephoneNumberType.Cell)]
    [TestCase("fax", TelephoneNumberType.Fax)]
    [TestCase("pager", TelephoneNumberType.Pager)]
    [TestCase("textphone", TelephoneNumberType.TextPhone)]
    [TestCase("main-number", TelephoneNumberType.MainNumber)]
    [TestCase("modem", TelephoneNumberType.Modem)]
    [TestCase("isdn", TelephoneNumberType.ISDN)]
    public void ParseTelephoneNumberType_KnownKeys_ReturnsType(string key, TelephoneNumberType expected)
    {
        SharedParsers.ParseTelephoneNumberType(key).Must().Be(expected);
    }

    [Test]
    public void ParseTelephoneNumberType_Unknown_ReturnsNull()
    {
        SharedParsers.ParseTelephoneNumberType("satphone").Must().BeNull();
    }

    [Test]
    public void DecodeQuotedPrintable_Empty_ReturnsEmpty()
    {
        SharedParsers.DecodeQuotedPrintable(string.Empty).Must().Be(string.Empty);
    }

    [Test]
    public void DecodeQuotedPrintable_HexByte_Decodes()
    {
        SharedParsers.DecodeQuotedPrintable("=41=42").Must().Be("AB");
    }

    [Test]
    public void DecodeQuotedPrintable_InvalidHex_KeepsEqualsSignAsByte()
    {
        var result = SharedParsers.DecodeQuotedPrintable("=ZZ");
        result.Must().NotBeNull();
        result.Length.Must().BeGreaterThan(0);
    }

    [Test]
    public void DecodeQuotedPrintable_PlainAscii_Passthrough()
    {
        SharedParsers.DecodeQuotedPrintable("hello").Must().Be("hello");
    }

    [Test]
    public void DecodeQuotedPrintable_TrailingEqualsWithoutHex_AppendsEqualsByte()
    {
        SharedParsers.DecodeQuotedPrintable("ab=").Must().Be("ab=");
    }

    [Test]
    public void DecodeQuotedPrintable_IncompleteHexAfterEquals_AppendsEquals()
    {
        SharedParsers.DecodeQuotedPrintable("x=A").Must().Be("x=A");
    }
}
