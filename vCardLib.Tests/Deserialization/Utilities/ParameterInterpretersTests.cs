using System;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class ParameterInterpretersTests
{
    [Test]
    public void ParsePreference_NumericOnly_ShouldParseCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "PREF=1" });
        ParameterInterpreters.ParsePreference(parameters, true).ShouldBe(1);
    }

    [Test]
    public void ParsePreference_NumericOnlyInvalid_ShouldReturnNull()
    {
        var parameters = VCardParameters.Parse(new[] { "PREF=abc" });
        ParameterInterpreters.ParsePreference(parameters, true).ShouldBeNull();
    }

    [Test]
    public void ParsePreference_Boolean_ShouldParseCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "PREF" });
        ParameterInterpreters.ParsePreference(parameters, false).ShouldBe(1);
    }

    [Test]
    public void ParsePreference_BooleanMissing_ShouldReturnNull()
    {
        var parameters = VCardParameters.Parse(new string[0]);
        ParameterInterpreters.ParsePreference(parameters, false).ShouldBeNull();
    }

    [Test]
    public void ParseTypeFlags_SingleType_ShouldParseCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "TYPE=WORK" });
        var result = ParameterInterpreters.ParseTypeFlags<EmailAddressType>(parameters, s => Enum.TryParse<EmailAddressType>(s, true, out var t) ? (EmailAddressType?)t : null);
        result.ShouldBe(EmailAddressType.Work);
    }

    [Test]
    public void ParseTypeFlags_MultipleTypes_ShouldCombineCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "TYPE=WORK,HOME" });
        var result = ParameterInterpreters.ParseTypeFlags<EmailAddressType>(parameters, s => Enum.TryParse<EmailAddressType>(s, true, out var t) ? (EmailAddressType?)t : null);
        result.ShouldBe(EmailAddressType.Work | EmailAddressType.Home);
    }

    [Test]
    public void ParseTypeFlags_BareTokens_ShouldParseCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "WORK", "HOME" });
        var result = ParameterInterpreters.ParseTypeFlags<EmailAddressType>(parameters, s => Enum.TryParse<EmailAddressType>(s, true, out var t) ? (EmailAddressType?)t : null);
        result.ShouldBe(EmailAddressType.Work | EmailAddressType.Home);
    }

    [Test]
    public void ParseStringParameter_QuotedInput_ShouldTrimAndStripQuotes()
    {
        var parameters = VCardParameters.Parse(new[] { "LABEL=\"My Label  \"" });
        ParameterInterpreters.ParseStringParameter(parameters, "LABEL").ShouldBe("My Label");
    }

    [Test]
    public void ParseStringParameter_MissingParameter_ShouldReturnNull()
    {
        var parameters = VCardParameters.Parse(new string[0]);
        ParameterInterpreters.ParseStringParameter(parameters, "LABEL").ShouldBeNull();
    }
}
