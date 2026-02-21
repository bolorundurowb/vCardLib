using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Deserialization.Utilities;

namespace vCardLib.Tests.Deserialization.Utilities;

[TestFixture]
public class VCardParametersTests
{
    [Test]
    public void Parse_EmptyMetadata_ReturnsEmptyParameters()
    {
        var parameters = VCardParameters.Parse(System.Array.Empty<string>());
        parameters.ContainsKey("TYPE").ShouldBeFalse();
    }

    [Test]
    public void Parse_SingleType_ReturnsCorrectValue()
    {
        var parameters = VCardParameters.Parse(new[] { "TYPE=WORK" });
        parameters.GetAll("TYPE").ShouldContain("WORK");
        parameters.GetFirst("TYPE").ShouldBe("WORK");
    }

    [Test]
    public void Parse_MultipleTypes_ReturnsAllValues()
    {
        var parameters = VCardParameters.Parse(new[] { "TYPE=home", "TYPE=blog" });
        var values = parameters.GetAll("TYPE").ToList();
        values.Count.ShouldBe(2);
        values.ShouldContain("home");
        values.ShouldContain("blog");
    }

    [Test]
    public void Parse_Preference_ReturnsFirstValue()
    {
        var parameters = VCardParameters.Parse(new[] { "PREF=1" });
        parameters.GetFirst("PREF").ShouldBe("1");
    }

    [Test]
    public void Parse_BareToken_ReturnsTokenAsKeyAndValue()
    {
        var parameters = VCardParameters.Parse(new[] { "WORK" });
        parameters.ContainsKey("WORK").ShouldBeTrue();
        parameters.GetFirst("WORK").ShouldBe("WORK");
    }

    [Test]
    public void Parse_CaseInsensitivity_MatchesCorrectly()
    {
        var parameters = VCardParameters.Parse(new[] { "TYPE=WORK" });
        parameters.GetAll("type").ShouldContain("WORK");
        parameters.GetFirst("type").ShouldBe("WORK");
        parameters.ContainsKey("type").ShouldBeTrue();
    }
}
