using System;
using System.Collections.Generic;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserialization;

internal static class V2DeserializerRegistry
{
    private static readonly Dictionary<string, Func<IFieldDeserializer>> Factories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["N"] = () => new DeserializerAdapter<Name>("N",
            line => ((IV2FieldDeserializer<Name>)new NameFieldDeserializer()).Read(line),
            (card, val) => card.Name = val),

        ["FN"] = () => new DeserializerAdapter<string>("FN",
            line => ((IV2FieldDeserializer<string>)new FormattedNameDeserializer()).Read(line),
            (card, val) => card.FormattedName = val),

        ["NICKNAME"] = () => new DeserializerAdapter<string>("NICKNAME",
            line => ((IV2FieldDeserializer<string>)new NicknameFieldDeserializer()).Read(line),
            (card, val) => card.NickName = val),

        ["NOTE"] = () => new DeserializerAdapter<string>("NOTE",
            line => ((IV2FieldDeserializer<string>)new NoteFieldDeserializer()).Read(line),
            (card, val) => card.Note = val),

        ["UID"] = () => new DeserializerAdapter<string>("UID",
            line => ((IV2FieldDeserializer<string>)new UidFieldDeserializer()).Read(line),
            (card, val) => card.Uid = val),

        ["URL"] = () => new DeserializerAdapter<Url>("URL",
            line => ((IV2FieldDeserializer<Url>)new UrlFieldDeserializer()).Read(line),
            (card, val) => card.Url = val),

        ["TZ"] = () => new DeserializerAdapter<string>("TZ",
            line => ((IV2FieldDeserializer<string>)new TimezoneFieldDeserializer()).Read(line),
            (card, val) => card.Timezone = val),

        ["GEO"] = () => new DeserializerAdapter<Geo>("GEO",
            line => ((IV2FieldDeserializer<Geo>)new GeoFieldDeserializer()).Read(line),
            (card, val) => card.Geo = val),

        ["ORG"] = () => new DeserializerAdapter<Organization?>("ORG",
            line => ((IV2FieldDeserializer<Organization?>)new OrganizationFieldDeserializer()).Read(line),
            (card, val) => card.Organization = val),

        ["TITLE"] = () => new DeserializerAdapter<string>("TITLE",
            line => ((IV2FieldDeserializer<string>)new TitleFieldDeserializer()).Read(line),
            (card, val) => card.Title = val),

        ["KIND"] = () => new DeserializerAdapter<ContactKind?>("KIND",
            line => ((IV2FieldDeserializer<ContactKind?>)new KindFieldDeserializer()).Read(line),
            (card, val) => card.Kind = val),

        ["GENDER"] = () => new DeserializerAdapter<Gender?>("GENDER",
            line => ((IV2FieldDeserializer<Gender?>)new GenderFieldDeserializer()).Read(line),
            (card, val) => card.Gender = val),

        ["REV"] = () => new DeserializerAdapter<DateTime?>("REV",
            line => ((IV2FieldDeserializer<DateTime?>)new RevisionFieldDeserializer()).Read(line),
            (card, val) => card.Revision = val),

        ["LANG"] = () => new DeserializerAdapter<Language?>("LANG",
            line => ((IV2FieldDeserializer<Language?>)new LanguageFieldDeserializer()).Read(line),
            (card, val) => card.Language = val),

        ["BDAY"] = () => new DeserializerAdapter<DateTime?>("BDAY",
            line => ((IV2FieldDeserializer<DateTime?>)new BirthdayFieldDeserializer()).Read(line),
            (card, val) => card.BirthDay = val),

        ["ANNIVERSARY"] = () => new DeserializerAdapter<DateTime?>("ANNIVERSARY",
            line => ((IV2FieldDeserializer<DateTime?>)new AnniversaryFieldDeserializer()).Read(line),
            (card, val) => card.Anniversary = val),

        ["PHOTO"] = () => new DeserializerAdapter<Photo>("PHOTO",
            line => ((IV2FieldDeserializer<Photo>)new PhotoFieldDeserializer()).Read(line),
            (card, val) => card.Photos.Add(val)),

        ["AGENT"] = () => new DeserializerAdapter<string>("AGENT",
            line => ((IV2FieldDeserializer<string>)new AgentFieldDeserializer()).Read(line),
            (card, val) => card.Agent = val),

        ["MAILER"] = () => new DeserializerAdapter<string>("MAILER",
            line => ((IV2FieldDeserializer<string>)new MailerFieldDeserializer()).Read(line),
            (card, val) => card.Mailer = val),

        ["CATEGORIES"] = () => new DeserializerAdapter<List<string>>("CATEGORIES",
            line => ((IV2FieldDeserializer<List<string>>)new CategoriesFieldDeserializer()).Read(line),
            (card, val) => card.Categories = val),

        ["MEMBER"] = () => new DeserializerAdapter<string>("MEMBER",
            line => ((IV2FieldDeserializer<string>)new MemberFieldDeserializer()).Read(line),
            (card, val) => card.Members.Add(val)),

        ["TEL"] = () => new DeserializerAdapter<TelephoneNumber>("TEL",
            line => ((IV2FieldDeserializer<TelephoneNumber>)new TelephoneNumberFieldDeserializer()).Read(line),
            (card, val) => card.PhoneNumbers.Add(val)),

        ["EMAIL"] = () => new DeserializerAdapter<EmailAddress>("EMAIL",
            line => ((IV2FieldDeserializer<EmailAddress>)new EmailAddressFieldDeserializer()).Read(line),
            (card, val) => card.EmailAddresses.Add(val)),

        ["ADR"] = () => new DeserializerAdapter<Address>("ADR",
            line => ((IV2FieldDeserializer<Address>)new AddressFieldDeserializer()).Read(line),
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
