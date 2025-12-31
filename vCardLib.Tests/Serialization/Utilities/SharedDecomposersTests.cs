using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Tests.Serialization.Utilities;

[TestFixture]
public class SharedDecomposersTests
{
    [TestCase(AddressType.Domestic, "dom")]
    [TestCase(AddressType.Home, "home")]
    [TestCase(AddressType.International, "intl")]
    [TestCase(AddressType.Parcel, "parcel")]
    [TestCase(AddressType.Postal, "postal")]
    [TestCase(AddressType.Work, "work")]
    public void DecomposeAddressType_ValidInput_ReturnsExpected(AddressType input, string expected)
    {
        input.DecomposeAddressType().ShouldBe(expected);
    }

    [Test]
    public void DecomposeAddressType_InvalidInput_ThrowsArgumentException()
    {
        var input = (AddressType)999;
        Should.Throw<ArgumentException>(() => input.DecomposeAddressType());
    }

    [TestCase(EmailAddressType.Work, "work")]
    [TestCase(EmailAddressType.Internet, "internet")]
    [TestCase(EmailAddressType.Home, "home")]
    [TestCase(EmailAddressType.Aol, "aol")]
    [TestCase(EmailAddressType.Applelink, "applelink")]
    [TestCase(EmailAddressType.IbmMail, "ibmmail")]
    [TestCase(EmailAddressType.Preferred, "pref")]
    public void DecomposeEmailAddressType_ValidInput_ReturnsExpected(EmailAddressType input, string expected)
    {
        input.DecomposeEmailAddressType().ShouldBe(expected);
    }

    [Test]
    public void DecomposeEmailAddressType_InvalidInput_ThrowsArgumentException()
    {
        var input = (EmailAddressType)999;
        Should.Throw<ArgumentException>(() => input.DecomposeEmailAddressType());
    }

    [TestCase(BiologicalSex.Male, "M")]
    [TestCase(BiologicalSex.Female, "F")]
    [TestCase(BiologicalSex.Other, "O")]
    [TestCase(BiologicalSex.None, "N")]
    [TestCase(BiologicalSex.Unknown, "U")]
    public void DecomposeBiologicalSex_ValidInput_ReturnsExpected(BiologicalSex input, string expected)
    {
        input.DecomposeBiologicalSex().ShouldBe(expected);
    }

    [Test]
    public void DecomposeBiologicalSex_InvalidInput_ThrowsArgumentException()
    {
        var input = (BiologicalSex)999;
        Should.Throw<ArgumentException>(() => input.DecomposeBiologicalSex());
    }

    [TestCase(ContactKind.Individual, "individual")]
    [TestCase(ContactKind.Group, "group")]
    [TestCase(ContactKind.Organization, "org")]
    [TestCase(ContactKind.Location, "location")]
    public void DecomposeContactKind_ValidInput_ReturnsExpected(ContactKind input, string expected)
    {
        input.DecomposeContactKind().ShouldBe(expected);
    }

    [Test]
    public void DecomposeContactKind_InvalidInput_ThrowsArgumentException()
    {
        var input = (ContactKind)999;
        Should.Throw<ArgumentException>(() => input.DecomposeContactKind());
    }

    [TestCase(TelephoneNumberType.Voice, "voice")]
    [TestCase(TelephoneNumberType.Text, "text")]
    [TestCase(TelephoneNumberType.Fax, "fax")]
    [TestCase(TelephoneNumberType.Cell, "cell")]
    [TestCase(TelephoneNumberType.Video, "video")]
    [TestCase(TelephoneNumberType.Pager, "pager")]
    [TestCase(TelephoneNumberType.TextPhone, "textphone")]
    [TestCase(TelephoneNumberType.Home, "home")]
    [TestCase(TelephoneNumberType.MainNumber, "main-number")]
    [TestCase(TelephoneNumberType.Work, "work")]
    [TestCase(TelephoneNumberType.BBS, "bbs")]
    [TestCase(TelephoneNumberType.Modem, "modem")]
    [TestCase(TelephoneNumberType.Car, "car")]
    [TestCase(TelephoneNumberType.ISDN, "isdn")]
    [TestCase(TelephoneNumberType.PCS, "pcs")]
    [TestCase(TelephoneNumberType.Preferred, "pref")]
    public void DecomposeTelephoneNumberType_ValidInput_ReturnsExpected(TelephoneNumberType input, string expected)
    {
        input.DecomposeTelephoneNumberType().ShouldBe(expected);
    }

    [Test]
    public void DecomposeTelephoneNumberType_InvalidInput_ThrowsArgumentException()
    {
        var input = (TelephoneNumberType)999;
        Should.Throw<ArgumentException>(() => input.DecomposeTelephoneNumberType());
    }
}
