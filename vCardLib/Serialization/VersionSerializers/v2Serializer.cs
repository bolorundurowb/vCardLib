using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serialization.FieldSerializers;
using vCardLib.Serialization.Interfaces;

namespace vCardLib.Serialization.VersionSerializers;

internal sealed class V2Serializer
{
    private readonly Dictionary<string, IFieldSerializer> _fieldSerializers;

    public V2Serializer(Dictionary<string, IFieldSerializer> fieldSerializers) => _fieldSerializers = fieldSerializers;

    public string Serialize(vCard card)
    {
        var builder = new StringBuilder();

        builder.Append(FieldKeyConstants.StartToken);
        builder.AppendLine(VersionFieldSerializer.Write(vCardVersion.v2));

        if (card.Name != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Name>)_fieldSerializers["N"]).Write(card.Name.Value)
            );

        if (card.FormattedName != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["FN"]).Write(card.FormattedName)
            );

        if (card.NickName != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["NICKNAME"]).Write(card.NickName)
            );

        if (card.Note != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["NOTE"]).Write(card.Note)
            );

        if (card.Uid != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["UID"]).Write(card.Uid)
            );

        if (card.Url != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["URL"]).Write(card.Url)
            );

        if (card.Timezone != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["TZ"]).Write(card.Timezone)
            );

        if (card.Geo != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Geo>)_fieldSerializers["GEO"]).Write(card.Geo.Value)
            );

        if (card.Organization != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Organization>)_fieldSerializers["ORG"]).Write(card.Organization.Value)
            );

        if (card.Title != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["TITLE"]).Write(card.Title)
            );

        if (card.Kind != null)
            builder.AppendLine(
                ((IV2FieldSerializer<ContactKind>)_fieldSerializers["KIND"]).Write(card.Kind.Value)
            );

        if (card.Gender != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Gender>)_fieldSerializers["GENDER"]).Write(card.Gender.Value)
            );

        if (card.Revision != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)_fieldSerializers["REV"]).Write(card.Revision.Value)
            );

        if (card.Language != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Language>)_fieldSerializers["LANG"]).Write(card.Language.Value)
            );

        if (card.Anniversary != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)_fieldSerializers["ANNIVERSARY"]).Write(card.Anniversary.Value)
            );

        if (card.BirthDay != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)_fieldSerializers["BIRTHDAY"]).Write(card.BirthDay.Value)
            );

        if (card.Logo != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Photo>)_fieldSerializers["LOGO"]).Write(card.Logo.Value)
            );

        if (card.Agent != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["AGENT"]).Write(card.Agent)
            );

        if (card.Mailer != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)_fieldSerializers["MAILER"]).Write(card.Mailer)
            );

        if (card.Categories.Any())
            builder.AppendLine(
                ((IV2FieldSerializer<List<string>>)_fieldSerializers["CATEGORIES"]).Write(card.Categories)
            );

        if (card.Members.Any())
            builder.AppendLine(
                ((IV2FieldSerializer<List<string>>)_fieldSerializers["MEMBER"]).Write(card.Members)
            );

        if (card.PhoneNumbers.Any())
            foreach (var phoneNumber in card.PhoneNumbers)
                builder.AppendLine(
                    ((IV2FieldSerializer<TelephoneNumber>)_fieldSerializers["TEL"]).Write(phoneNumber)
                );

        if (card.EmailAddresses.Any())
            foreach (var emailAddress in card.EmailAddresses)
                builder.AppendLine(
                    ((IV2FieldSerializer<EmailAddress>)_fieldSerializers["EMAIL"]).Write(emailAddress)
                );

        if (card.Photos.Any())
            foreach (var photo in card.Photos)
                builder.AppendLine(
                    ((IV2FieldSerializer<Photo>)_fieldSerializers["PHOTO"]).Write(photo)
                );

        if (card.Addresses.Any())
            foreach (var address in card.Addresses)
                builder.AppendLine(
                    ((IV2FieldSerializer<Address>)_fieldSerializers["ADR"]).Write(address)
                );

        if (card.CustomFields.Any())
            foreach (var customField in card.CustomFields)
                builder.AppendLine(
                    ((IV2FieldSerializer<KeyValuePair<string, string>>)_fieldSerializers["UNKNOWN"]).Write(customField)
                );

        builder.Append(FieldKeyConstants.EndToken);

        return builder.ToString();
    }
}
