using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shouldly;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization;

namespace vCardLib.Tests.Serialization;

[TestFixture]
public class vCardSerializerTests
{
    [Test]
    public void Serialize_SingleCard_ReturnsVCardString()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card);

        result.ShouldContain("BEGIN:VCARD");
        result.ShouldContain("VERSION:2.1");
        result.ShouldContain("FN:John Doe");
        result.ShouldContain("END:VCARD");
    }

    [Test]
    public void Serialize_MultipleCards_ReturnsVCardString()
    {
        var cards = new List<vCard>
        {
            new(vCardVersion.v3) { FormattedName = "John Doe" },
            new(vCardVersion.v3) { FormattedName = "Jane Doe" }
        };

        var result = vCardSerializer.Serialize(cards);

        result.ShouldContain("BEGIN:VCARD");
        result.ShouldContain("VERSION:3.0");
        result.ShouldContain("FN:John Doe");
        result.ShouldContain("FN:Jane Doe");
        result.ShouldContain("END:VCARD");
        
        // Count occurrences of BEGIN:VCARD
        var count = System.Text.RegularExpressions.Regex.Matches(result, "BEGIN:VCARD").Count;
        count.ShouldBe(2);
    }

    [Test]
    public void Serialize_EmptyCollection_ReturnsEmptyString()
    {
        var result = vCardSerializer.Serialize(Enumerable.Empty<vCard>());
        result.ShouldBe(string.Empty);
    }

    [Test]
    public void Serialize_OverrideVersion_UsesSpecifiedVersion()
    {
        var card = new vCard(vCardVersion.v2)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card, vCardVersion.v4);

        result.ShouldContain("VERSION:4.0");
    }

    [Test]
    public void Serialize_V3_ReturnsCorrectVersion()
    {
        var card = new vCard(vCardVersion.v3)
        {
            FormattedName = "John Doe"
        };

        var result = vCardSerializer.Serialize(card);

        result.ShouldContain("VERSION:3.0");
    }
}
