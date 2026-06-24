using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;
using vCardLib.Serialization.Utilities;

namespace vCardLib.Serialization.VersionSerializers;

internal sealed class V2Serializer
{
    private static readonly (Func<vCard, bool> HasData, Func<IFieldSerializer> Factory)[] Fields =
    [
        (c => c.Name != null, () => new SerializerAdapter<Name>(
            c => c.Name!.Value,
            data => ((IV2FieldSerializer<Name>)new NameFieldSerializer()).Write(data))),
        (c => c.FormattedName != null, () => new SerializerAdapter<string>(
            c => c.FormattedName,
            data => ((IV2FieldSerializer<string>)new FormattedNameSerializer()).Write(data))),
        (c => c.NickName != null, () => new SerializerAdapter<string>(
            c => c.NickName,
            data => ((IV2FieldSerializer<string>)new NicknameFieldSerializer()).Write(data))),
        (c => c.Note != null, () => new SerializerAdapter<string>(
            c => c.Note,
            data => ((IV2FieldSerializer<string>)new NoteFieldSerializer()).Write(data))),
        (c => c.Uid != null, () => new SerializerAdapter<string>(
            c => c.Uid,
            data => ((IV2FieldSerializer<string>)new UidFieldSerializer()).Write(data))),
        (c => c.Url != null, () => new SerializerAdapter<Url>(
            c => c.Url!.Value,
            data => ((IV2FieldSerializer<Url>)new UrlFieldSerializer()).Write(data))),
        (c => c.Timezone != null, () => new SerializerAdapter<string>(
            c => c.Timezone,
            data => ((IV2FieldSerializer<string>)new TimezoneFieldSerializer()).Write(data))),
        (c => c.Geo != null, () => new SerializerAdapter<Geo>(
            c => c.Geo!.Value,
            data => ((IV2FieldSerializer<Geo>)new GeoFieldSerializer()).Write(data))),
        (c => c.Organization != null, () => new SerializerAdapter<Organization>(
            c => c.Organization!.Value,
            data => ((IV2FieldSerializer<Organization>)new OrganizationFieldSerializer()).Write(data))),
        (c => c.Title != null, () => new SerializerAdapter<string>(
            c => c.Title,
            data => ((IV2FieldSerializer<string>)new TitleFieldSerializer()).Write(data))),
        (c => c.Kind != null, () => new SerializerAdapter<ContactKind>(
            c => c.Kind!.Value,
            data => ((IV2FieldSerializer<ContactKind>)new KindSerializer()).Write(data))),
        (c => c.Gender != null, () => new SerializerAdapter<Gender>(
            c => c.Gender!.Value,
            data => ((IV2FieldSerializer<Gender>)new GenderFieldSerializer()).Write(data))),
        (c => c.Revision != null, () => new SerializerAdapter<DateTime>(
            c => c.Revision!.Value,
            data => ((IV2FieldSerializer<DateTime>)new RevisionFieldSerializer()).Write(data))),
        (c => c.Language != null, () => new SerializerAdapter<Language>(
            c => c.Language!.Value,
            data => ((IV2FieldSerializer<Language>)new LanguageFieldSerializer()).Write(data))),
        (c => c.Anniversary != null, () => new SerializerAdapter<DateTime>(
            c => c.Anniversary!.Value,
            data => ((IV2FieldSerializer<DateTime>)new AnniversaryFieldSerializer()).Write(data))),
        (c => c.BirthDay != null, () => new SerializerAdapter<DateTime>(
            c => c.BirthDay!.Value,
            data => ((IV2FieldSerializer<DateTime>)new BirthdayFieldSerializer()).Write(data))),
        (c => c.Logo != null, () => new SerializerAdapter<Photo>(
            c => c.Logo!.Value,
            data => ((IV2FieldSerializer<Photo>)new LogoFieldSerializer()).Write(data))),
        (c => c.Agent != null, () => new SerializerAdapter<string>(
            c => c.Agent,
            data => ((IV2FieldSerializer<string>)new AgentFieldSerializer()).Write(data))),
        (c => c.Mailer != null, () => new SerializerAdapter<string>(
            c => c.Mailer,
            data => ((IV2FieldSerializer<string>)new MailerFieldSerializer()).Write(data))),
        (c => c.Categories.Any(), () => new SerializerAdapter<List<string>>(
            c => c.Categories,
            data => ((IV2FieldSerializer<List<string>>)new CategoriesFieldSerializer()).Write(data))),
        (c => c.PhoneNumbers.Any(), () => new CollectionSerializerAdapter<TelephoneNumber>(
            c => c.PhoneNumbers,
            data => ((IV2FieldSerializer<TelephoneNumber>)new TelephoneNumberFieldSerializer()).Write(data))),
        (c => c.EmailAddresses.Any(), () => new CollectionSerializerAdapter<EmailAddress>(
            c => c.EmailAddresses,
            data => ((IV2FieldSerializer<EmailAddress>)new EmailAddressFieldSerializer()).Write(data))),
        (c => c.Photos.Any(), () => new CollectionSerializerAdapter<Photo>(
            c => c.Photos,
            data => ((IV2FieldSerializer<Photo>)new PhotoFieldSerializer()).Write(data))),
        (c => c.Addresses.Any(), () => new CollectionSerializerAdapter<Address>(
            c => c.Addresses,
            data => ((IV2FieldSerializer<Address>)new AddressFieldSerializer()).Write(data))),
        (c => c.CustomFields.Any(), () => new CollectionSerializerAdapter<KeyValuePair<string, string>>(
            c => c.CustomFields,
            data => ((IV2FieldSerializer<KeyValuePair<string, string>>)new CustomFieldSerializer()).Write(data))),
    ];

    public string Serialize(vCard card)
    {
        var builder = new StringBuilder();

        VCardContentLineFormatter.AppendCrlf(builder, FieldKeyConstants.StartToken);
        VCardContentLineFormatter.AppendCrlf(builder, VersionFieldSerializer.Write(vCardVersion.v2));

        foreach (var (hasData, factory) in Fields)
        {
            if (!hasData(card)) continue;
            var serializer = factory();
            foreach (var line in serializer.Serialize(card) ?? Enumerable.Empty<string>())
                VCardContentLineFormatter.AppendFoldedContentLine(builder, line);
        }

        VCardContentLineFormatter.AppendCrlf(builder, FieldKeyConstants.EndToken);

        return builder.ToString();
    }
}
