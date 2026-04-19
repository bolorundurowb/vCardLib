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

internal sealed class V4Serializer
{
    private readonly Dictionary<string, IFieldSerializer> _fieldSerializers;

    public V4Serializer(Dictionary<string, IFieldSerializer> fieldSerializers) => _fieldSerializers = fieldSerializers;

    public string Serialize(vCard card)
    {
        var builder = new StringBuilder();

        VCardContentLineFormatter.AppendCrlf(builder, FieldKeyConstants.StartToken);
        VCardContentLineFormatter.AppendCrlf(builder, VersionFieldSerializer.Write(vCardVersion.v4));

        if (card.Name != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Name>)_fieldSerializers["N"]).Write(card.Name.Value)
            );

        if (card.FormattedName != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["FN"]).Write(card.FormattedName)
            );

        if (card.NickName != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["NICKNAME"]).Write(card.NickName)
            );

        if (card.Note != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["NOTE"]).Write(card.Note)
            );

        if (card.Uid != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["UID"]).Write(card.Uid)
            );

        if (card.Url != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Url>)_fieldSerializers["URL"]).Write(card.Url.Value)
            );

        if (card.Timezone != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["TZ"]).Write(card.Timezone)
            );

        if (card.Geo != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Geo>)_fieldSerializers["GEO"]).Write(card.Geo.Value)
            );

        if (card.Organization != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Organization>)_fieldSerializers["ORG"]).Write(card.Organization.Value)
            );

        if (card.Title != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["TITLE"]).Write(card.Title)
            );

        if (card.Kind != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<ContactKind>)_fieldSerializers["KIND"]).Write(card.Kind.Value)
            );

        if (card.Gender != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Gender>)_fieldSerializers["GENDER"]).Write(card.Gender.Value)
            );

        if (card.Revision != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<DateTime>)_fieldSerializers["REV"]).Write(card.Revision.Value)
            );

        if (card.Language != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Language>)_fieldSerializers["LANG"]).Write(card.Language.Value)
            );

        if (card.Anniversary != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<DateTime>)_fieldSerializers["ANNIVERSARY"]).Write(card.Anniversary.Value)
            );

        if (card.BirthDay != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<DateTime>)_fieldSerializers["BDAY"]).Write(card.BirthDay.Value)
            );

        if (card.Logo != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<Photo>)_fieldSerializers["LOGO"]).Write(card.Logo.Value)
            );

        if (card.Agent != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["AGENT"]).Write(card.Agent)
            );

        if (card.Mailer != null)
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<string>)_fieldSerializers["MAILER"]).Write(card.Mailer)
            );

        if (card.Categories.Any())
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<List<string>>)_fieldSerializers["CATEGORIES"]).Write(card.Categories)
            );

        if (card.Members.Any())
            VCardContentLineFormatter.AppendFoldedContentLine(builder,
                ((IV4FieldSerializer<List<string>>)_fieldSerializers["MEMBER"]).Write(card.Members)
            );

        if (card.PhoneNumbers.Any())
            foreach (var phoneNumber in card.PhoneNumbers)
                VCardContentLineFormatter.AppendFoldedContentLine(builder,
                    ((IV4FieldSerializer<TelephoneNumber>)_fieldSerializers["TEL"]).Write(phoneNumber)
                );

        if (card.EmailAddresses.Any())
            foreach (var emailAddress in card.EmailAddresses)
                VCardContentLineFormatter.AppendFoldedContentLine(builder,
                    ((IV4FieldSerializer<EmailAddress>)_fieldSerializers["EMAIL"]).Write(emailAddress)
                );

        if (card.Photos.Any())
            foreach (var photo in card.Photos)
                VCardContentLineFormatter.AppendFoldedContentLine(builder,
                    ((IV4FieldSerializer<Photo>)_fieldSerializers["PHOTO"]).Write(photo)
                );

        if (card.Addresses.Any())
            foreach (var address in card.Addresses)
                VCardContentLineFormatter.AppendFoldedContentLine(builder,
                    ((IV4FieldSerializer<Address>)_fieldSerializers["ADR"]).Write(address)
                );

        if (card.CustomFields.Any())
            foreach (var customField in card.CustomFields)
                VCardContentLineFormatter.AppendFoldedContentLine(builder,
                    ((IV4FieldSerializer<KeyValuePair<string, string>>)_fieldSerializers["UNKNOWN"]).Write(customField)
                );

        VCardContentLineFormatter.AppendCrlf(builder, FieldKeyConstants.EndToken);

        return builder.ToString();
    }
}
