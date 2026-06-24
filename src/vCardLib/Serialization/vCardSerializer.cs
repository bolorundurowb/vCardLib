using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.VersionSerializers;

namespace vCardLib.Serialization;

// ReSharper disable once InconsistentNaming
public static class vCardSerializer
{
    public static string Serialize(vCard card, vCardVersion? overrideVersion = null)
    {
        var version = overrideVersion ?? card.Version;

        if (version is vCardVersion.v2)
            return new V2Serializer().Serialize(card);

        if (version is vCardVersion.v3)
            return new V3Serializer().Serialize(card);

        if (version is vCardVersion.v4)
            return new V4Serializer().Serialize(card);

        throw new ArgumentException("Unknown version", nameof(version));
    }

    public static string Serialize(IEnumerable<vCard> cards, vCardVersion? overrideVersion = null)
    {
        var cardList = cards.ToList();

        if (cardList.Count == 0)
            return string.Empty;

        var version = overrideVersion ?? cardList.First().Version;
        var builder = new StringBuilder();

        switch (version)
        {
            case vCardVersion.v2:
                {
                    var serializer = new V2Serializer();
                    foreach (var card in cardList)
                        builder.Append(serializer.Serialize(card));
                    break;
                }
            case vCardVersion.v3:
                {
                    var serializer = new V3Serializer();
                    foreach (var card in cardList)
                        builder.Append(serializer.Serialize(card));
                    break;
                }
            case vCardVersion.v4:
                {
                    var serializer = new V4Serializer();
                    foreach (var card in cardList)
                        builder.Append(serializer.Serialize(card));
                    break;
                }
            default:
                throw new ArgumentException("Unknown version", nameof(version));
        }

        return builder.ToString();
    }
}
