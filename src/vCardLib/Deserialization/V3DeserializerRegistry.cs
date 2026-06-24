using System;
using System.Collections.Generic;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserialization;

internal static class V3DeserializerRegistry
{
    private static readonly Dictionary<string, Func<IFieldDeserializer>> Factories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["N"] = () => new DeserializerAdapter<Name>("N",
            line => ((IV3FieldDeserializer<Name>)new NameFieldDeserializer()).Read(line),
            (card, val) => card.Name = val),

        ["FN"] = () => new DeserializerAdapter<string>("FN",
            line => ((IV3FieldDeserializer<string>)new FormattedNameDeserializer()).Read(line),
            (card, val) => card.FormattedName = val),

        ["NICKNAME"] = () => new DeserializerAdapter<string>("NICKNAME",
            line => ((IV3FieldDeserializer<string>)new NicknameFieldDeserializer()).Read(line),
            (card, val) => card.NickName = val),

        ["NOTE"] = () => new DeserializerAdapter<string>("NOTE",
            line => ((IV3FieldDeserializer<string>)new NoteFieldDeserializer()).Read(line),
            (card, val) => card.Note = val),

        ["UID"] = () => new DeserializerAdapter<string>("UID",
            line => ((IV3FieldDeserializer<string>)new UidFieldDeserializer()).Read(line),
            (card, val) => card.Uid = val),

        ["URL"] = () => new DeserializerAdapter<Url>("URL",
            line => ((IV3FieldDeserializer<Url>)new UrlFieldDeserializer()).Read(line),
            (card, val) => card.Url = val),

        ["TZ"] = () => new DeserializerAdapter<string>("TZ",
            line => ((IV3FieldDeserializer<string>)new TimezoneFieldDeserializer()).Read(line),
            (card, val) => card.Timezone = val),

        ["GEO"] = () => new DeserializerAdapter<Geo>("GEO",
            line => ((IV3FieldDeserializer<Geo>)new GeoFieldDeserializer()).Read(line),
            (card, val) => card.Geo = val),

        ["ORG"] = () => new DeserializerAdapter<Organization?>("ORG",
            line => ((IV3FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(line),
            (card, val) => card.Organization = val),

        ["TITLE"] = () => new DeserializerAdapter<string>("TITLE",
            line => ((IV3FieldDeserializer<string>)new TitleFieldDeserializer()).Read(line),
            (card, val) => card.Title = val),

        ["KIND"] = () => new DeserializerAdapter<ContactKind?>("KIND",
            line => ((IV3FieldDeserializer<ContactKind?>)new KindFieldDeserializer()).Read(line),
            (card, val) => card.Kind = val),

        ["GENDER"] = () => new DeserializerAdapter<Gender?>("GENDER",
            line => ((IV3FieldDeserializer<Gender?>)new GenderFieldDeserializer()).Read(line),
            (card, val) => card.Gender = val),

        ["REV"] = () => new DeserializerAdapter<DateTime?>("REV",
            line => ((IV3FieldDeserializer<DateTime?>)new RevisionFieldDeserializer()).Read(line),
            (card, val) => card.Revision = val),

        ["LANG"] = () => new DeserializerAdapter<Language?>("LANG",
            line => ((IV3FieldDeserializer<Language?>)new LanguageFieldDeserializer()).Read(line),
            (card, val) => card.Language = val),

        ["BDAY"] = () => new DeserializerAdapter<DateTime?>("BDAY",
            line => ((IV3FieldDeserializer<DateTime?>)new BirthdayFieldDeserializer()).Read(line),
            (card, val) => card.BirthDay = val),

        ["ANNIVERSARY"] = () => new DeserializerAdapter<DateTime?>("ANNIVERSARY",
            line => ((IV3FieldDeserializer<DateTime?>)new AnniversaryFieldDeserializer()).Read(line),
            (card, val) => card.Anniversary = val),

        ["PHOTO"] = () => new DeserializerAdapter<Photo>("PHOTO",
            line => ((IV3FieldDeserializer<Photo>)new PhotoFieldDeserializer()).Read(line),
            (card, val) => card.Photos.Add(val)),

        ["AGENT"] = () => new DeserializerAdapter<string>("AGENT",
            line => ((IV3FieldDeserializer<string>)new AgentFieldDeserializer()).Read(line),
            (card, val) => card.Agent = val),

        ["MAILER"] = () => new DeserializerAdapter<string>("MAILER",
            line => ((IV3FieldDeserializer<string>)new MailerFieldDeserializer()).Read(line),
            (card, val) => card.Mailer = val),

        ["CATEGORIES"] = () => new DeserializerAdapter<List<string>>("CATEGORIES",
            line => ((IV3FieldDeserializer<List<string>>)new CategoriesFieldDeserializer()).Read(line),
            (card, val) => card.Categories = val),

        ["MEMBER"] = () => new DeserializerAdapter<string>("MEMBER",
            line => ((IV3FieldDeserializer<string>)new MemberFieldDeserializer()).Read(line),
            (card, val) => card.Members.Add(val)),

        ["TEL"] = () => new DeserializerAdapter<TelephoneNumber>("TEL",
            line => ((IV3FieldDeserializer<TelephoneNumber>)new TelephoneNumberFieldDeserializer()).Read(line),
            (card, val) => card.PhoneNumbers.Add(val)),

        ["EMAIL"] = () => new DeserializerAdapter<EmailAddress>("EMAIL",
            line => ((IV3FieldDeserializer<EmailAddress>)new EmailAddressFieldDeserializer()).Read(line),
            (card, val) => card.EmailAddresses.Add(val)),

        ["ADR"] = () => new DeserializerAdapter<Address>("ADR",
            line => ((IV3FieldDeserializer<Address>)new AddressFieldDeserializer()).Read(line),
            (card, val) => card.Addresses.Add(val)),
    };

    private static readonly Dictionary<string, IFieldDeserializer> Cache = new(StringComparer.OrdinalIgnoreCase);

    public static IFieldDeserializer? Get(string key)
    {
        if (!Factories.TryGetValue(key, out var factory))
            return null;

        if (!Cache.TryGetValue(key, out var deserializer))
        {
            deserializer = factory();
            Cache[key] = deserializer;
        }

        return deserializer;
    }
}
