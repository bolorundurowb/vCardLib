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

internal sealed class v2Serializer
{
    private readonly Dictionary<string, IFieldSerializer> FieldSerializers;

    public v2Serializer(Dictionary<string, IFieldSerializer> fieldSerializers) => FieldSerializers = fieldSerializers;

    public string Serialize(vCard card)
    {
        var builder = new StringBuilder();
        
        builder.AppendLine(FieldKeyConstants.StartToken);
        builder.AppendLine(VersionFieldSerializer.Write(vCardVersion.v2));

        if (card.Name != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Name>)FieldSerializers["N"]).Write(card.Name.Value)
            );

        if (card.FormattedName != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["FN"]).Write(card.FormattedName)
            );

        if (card.NickName != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["NICKNAME"]).Write(card.NickName)
            );

        if (card.Note != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["NOTE"]).Write(card.Note)
            );

        if (card.Uid != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["UID"]).Write(card.Uid)
            );

        if (card.Url != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["URL"]).Write(card.Url)
            );

        if (card.Timezone != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["TZ"]).Write(card.Timezone)
            );

        if (card.Geo != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Geo>)FieldSerializers["GEO"]).Write(card.Geo.Value)
            );

        if (card.Organization != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Organization>)FieldSerializers["ORG"]).Write(card.Organization.Value)
            );

        if (card.Title != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["TITLE"]).Write(card.Title)
            );

        if (card.Kind != null)
            builder.AppendLine(
                ((IV2FieldSerializer<ContactKind>)FieldSerializers["KIND"]).Write(card.Kind.Value)
            );

        if (card.Gender != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Gender>)FieldSerializers["GENDER"]).Write(card.Gender.Value)
            );

        if (card.Revision != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)FieldSerializers["REV"]).Write(card.Revision.Value)
            );

        if (card.Language != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Language>)FieldSerializers["LANG"]).Write(card.Language.Value)
            );

        if (card.Anniversary != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)FieldSerializers["ANNIVERSARY"]).Write(card.Anniversary.Value)
            );

        if (card.BirthDay != null)
            builder.AppendLine(
                ((IV2FieldSerializer<DateTime>)FieldSerializers["BIRTHDAY"]).Write(card.BirthDay.Value)
            );

        if (card.Logo != null)
            builder.AppendLine(
                ((IV2FieldSerializer<Photo>)FieldSerializers["LOGO"]).Write(card.Logo.Value)
            );

        if (card.Agent != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["AGENT"]).Write(card.Agent)
            );

        if (card.Mailer != null)
            builder.AppendLine(
                ((IV2FieldSerializer<string>)FieldSerializers["MAILER"]).Write(card.Mailer)
            );

        if (card.Categories.Any())
            builder.AppendLine(
                ((IV2FieldSerializer<List<string>>)FieldSerializers["CATEGORIES"]).Write(card.Categories)
            );

        if (card.Members.Any())
            foreach (var member in card.Members)
                builder.AppendLine(
                    ((IV2FieldSerializer<string>)FieldSerializers["MEMBER"]).Write(member)
                );

        if (card.PhoneNumbers.Any())
            foreach (var phoneNumber in card.PhoneNumbers)
                builder.AppendLine(
                    ((IV2FieldSerializer<TelephoneNumber>)FieldSerializers["TEL"]).Write(phoneNumber)
                );

        if (card.EmailAddresses.Any())
            foreach (var emailAddress in card.EmailAddresses)
                builder.AppendLine(
                    ((IV2FieldSerializer<EmailAddress>)FieldSerializers["EMAIL"]).Write(emailAddress)
                );

        if (card.Photos.Any())
            foreach (var photo in card.Photos)
                builder.AppendLine(
                    ((IV2FieldSerializer<Photo>)FieldSerializers["PHOTO"]).Write(photo)
                );

        if (card.Addresses.Any())
            foreach (var address in card.Addresses)
                builder.AppendLine(
                    ((IV2FieldSerializer<Address>)FieldSerializers["ADR"]).Write(address)
                );

        if (card.CustomFields.Any())
            foreach (var customField in card.CustomFields)
                builder.AppendLine(
                    ((IV2FieldSerializer<KeyValuePair<string, string>>)FieldSerializers["UNKNOWN"]).Write(customField)
                );

        builder.AppendLine(FieldKeyConstants.EndToken);

        return builder.ToString();
    }
}
