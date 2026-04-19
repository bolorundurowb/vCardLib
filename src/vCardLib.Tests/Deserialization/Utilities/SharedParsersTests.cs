using System;
using NUnit.Framework;
using Shouldly;
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
        result.ShouldNotBeNull();
        result!.Value.Year.ShouldBe(1990);
        result.Value.Month.ShouldBe(5);
        result.Value.Day.ShouldBe(15);
    }

    [Test]
    public void ParseDate_YyyyMmDdThhMmSsZ_ReturnsUtc()
    {
        var result = SharedParsers.ParseDate("19900515T120000Z");
        result.ShouldNotBeNull();
        result!.Value.Hour.ShouldBe(12);
    }

    [Test]
    public void ParseDate_HhMmTime_ReturnsDateWithTimeOffset()
    {
        var result = SharedParsers.ParseDate("0630");
        result.ShouldNotBeNull();
    }

    [Test]
    public void ParseDate_Unrecognized_ReturnsNull()
    {
        SharedParsers.ParseDate("not-a-date").ShouldBeNull();
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
        key.ParseAddressType().ShouldBe(expected);
    }

    [Test]
    public void ParseAddressType_Unknown_ReturnsNull()
    {
        "unknown-type".ParseAddressType().ShouldBeNull();
    }

    [TestCase("internet", EmailAddressType.Internet)]
    [TestCase("pref", EmailAddressType.Preferred)]
    [TestCase("aol", EmailAddressType.Aol)]
    [TestCase("ibmmail", EmailAddressType.IbmMail)]
    [TestCase("applelink", EmailAddressType.Applelink)]
    public void ParseEmailAddressType_KnownKeys_ReturnsType(string key, EmailAddressType expected)
    {
        SharedParsers.ParseEmailAddressType(key).ShouldBe(expected);
    }

    [Test]
    public void ParseEmailAddressType_Unknown_ReturnsNull()
    {
        SharedParsers.ParseEmailAddressType("satellite").ShouldBeNull();
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
        SharedParsers.ParseTelephoneNumberType(key).ShouldBe(expected);
    }

    [Test]
    public void ParseTelephoneNumberType_Unknown_ReturnsNull()
    {
        SharedParsers.ParseTelephoneNumberType("satphone").ShouldBeNull();
    }

    [Test]
    public void DecodeQuotedPrintable_Empty_ReturnsEmpty()
    {
        SharedParsers.DecodeQuotedPrintable(string.Empty).ShouldBe(string.Empty);
    }

    [Test]
    public void DecodeQuotedPrintable_HexByte_Decodes()
    {
        SharedParsers.DecodeQuotedPrintable("=41=42").ShouldBe("AB");
    }

    [Test]
    public void DecodeQuotedPrintable_InvalidHex_KeepsEqualsSignAsByte()
    {
        var result = SharedParsers.DecodeQuotedPrintable("=ZZ");
        result.ShouldNotBeNull();
        result.Length.ShouldBeGreaterThan(0);
    }

    [Test]
    public void DecodeQuotedPrintable_PlainAscii_Passthrough()
    {
        SharedParsers.DecodeQuotedPrintable("hello").ShouldBe("hello");
    }

    [Test]
    public void DecodeQuotedPrintable_TrailingEqualsWithoutHex_AppendsEqualsByte()
    {
        SharedParsers.DecodeQuotedPrintable("ab=").ShouldBe("ab=");
    }

    [Test]
    public void DecodeQuotedPrintable_IncompleteHexAfterEquals_AppendsEquals()
    {
        SharedParsers.DecodeQuotedPrintable("x=A").ShouldBe("x=A");
    }
}
