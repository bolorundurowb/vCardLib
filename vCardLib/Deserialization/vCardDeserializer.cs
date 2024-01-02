using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCardLib.Constants;
using vCardLib.Deserialization.FieldDeserializers;
using vCardLib.Deserialization.Interfaces;
using vCardLib.Deserialization.Utilities;
using vCardLib.Enums;
using vCardLib.Extensions;
using vCardLib.Models;

namespace vCardLib.Deserialization;

// ReSharper disable once InconsistentNaming
public static class vCardDeserializer
{
    private static readonly Dictionary<string, IFieldDeserializer> FieldDeserializers = new()
    {
        { UnknownFieldDeserializer.Key, new UnknownFieldDeserializer() },
        { AddressFieldDeserializer.FieldKey, new AddressFieldDeserializer() },
    };

    public static IEnumerable<vCard> FromFile(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        if (File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        return FromContent(File.ReadAllText(filePath));
    }

    public static IEnumerable<vCard> FromStream(Stream stream)
    {
        var encoding = stream.GetEncoding();
        using var reader = new StreamReader(stream, encoding);
        var contents = reader.ReadToEnd();
        return FromContent(contents);
    }

    public static IEnumerable<vCard> FromContent(string vcardContents)
    {
        if (string.IsNullOrWhiteSpace(vcardContents))
            throw new ArgumentException("File is empty.", nameof(vcardContents));

        if (vcardContents.StartsWith(FieldKeyConstants.StartToken))
            throw new Exception($"A vCard must begin with '{FieldKeyConstants.StartToken}'.");

        if (vcardContents.EndsWith(FieldKeyConstants.EndToken))
            throw new Exception($"A vCard must end with '{FieldKeyConstants.EndToken}'.");

        if (vcardContents.Contains(FieldKeyConstants.VersionKey))
            throw new Exception($"A vCard must contain a '{FieldKeyConstants.VersionKey}'.");

        var cardGroups = SplitContent(vcardContents);

        foreach (var vcardContent in cardGroups)
            yield return Convert(vcardContent);
    }

    #region Private Helpers

    private static IEnumerable<string[]> SplitContent(string vcardContent)
    {
        using var reader = new StringReader(vcardContent);
        var response = new List<string>();

        while (reader.ReadLine()?.Trim() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            if (line.EqualsIgnoreCase("END:VCARD"))
            {
                yield return response.ToArray();
            }
            else if (line.EqualsIgnoreCase("BEGIN:VCARD"))
            {
                response.Clear();
            }
            else if (line.EndsWithIgnoreCase("BEGIN:VCARD"))
            {
                var nested = new StringBuilder(line);

                while (reader.ReadLine()?.Trim() is { } nestedLine && !nestedLine.EqualsIgnoreCase("END:VCARD"))
                    nested.AppendLine(nestedLine);

                response.Add(nested.ToString());
            }
            else
            {
                response.Add(line);
            }
        }
    }

    private static vCard Convert(string[] vcardContent)
    {
        var versionRow = vcardContent.FirstOrDefault(x => x.StartsWith(VersionDeserializer.FieldKey));

        if (versionRow == null)
            throw new ArgumentException("No version specified");

        var version = VersionDeserializer.Read(versionRow);

        if (version == vCardVersion.v2)
            return DeserializeV2(vcardContent);

        if (version == vCardVersion.v3)
            return DeserializeV3(vcardContent);

        if (version == vCardVersion.v4)
            return DeserializeV4(vcardContent);

        throw new ArgumentException($"Unsupported version: {version}");
    }

    private static vCard DeserializeV2(IReadOnlyCollection<string> vcardContent)
    {
        var vcard = new vCard(vCardVersion.v2);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Name>>(NameFieldDeserializer.FieldKey,
                out var rawName, out var nameDes))
            vcard.Name = nameDes!.Read(rawName!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(FormattedNameDeserializer.FieldKey,
                out var rawFormattedName, out var fnDes))
            vcard.FormattedName = fnDes!.Read(rawFormattedName!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(NicknameFieldDeserializer.FieldKey,
                out var rawNick, out var nickDes))
            vcard.NickName = nickDes!.Read(rawNick!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(NoteFieldDeserializer.FieldKey,
                out var rawNote, out var noteDes))
            vcard.Note = noteDes!.Read(rawNote!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(UrlFieldDeserializer.FieldKey,
                out var rawUrl, out var urlDes))
            vcard.Url = urlDes!.Read(rawUrl!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(TimezoneFieldDeserializer.FieldKey,
                out var rawTz, out var tzDes))
            vcard.Timezone = tzDes!.Read(rawTz!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Geo>>(GeoFieldDeserializer.FieldKey,
                out var rawGeo, out var geoDes))
            vcard.Geo = geoDes!.Read(rawGeo!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Organization>>(
                OrganizationFieldDeserializer.FieldKey,
                out var rawOrg, out var orgDes))
            vcard.Organization = orgDes!.Read(rawOrg!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(TitleFieldDeserializer.FieldKey,
                out var rawTitle, out var titleDes))
            vcard.Title = titleDes!.Read(rawTitle!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<ContactKind>>(KindFieldDeserializer.FieldKey,
                out var rawKind, out var kindDes))
            vcard.Kind = kindDes!.Read(rawKind!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Gender>>(GenderFieldDeserializer.FieldKey,
                out var rawGender, out var genderDes))
            vcard.Gender = genderDes!.Read(rawGender!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<DateTime>>(RevisionFieldDeserializer.FieldKey,
                out var rawRev, out var revDes))
            vcard.Revision = revDes!.Read(rawRev!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Language>>(LanguageFieldDeserializer.FieldKey,
                out var rawLang, out var langDes))
            vcard.Language = langDes!.Read(rawLang!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<DateTime>>(BirthdayFieldDeserializer.FieldKey,
                out var rawBday, out var bdayDes))
            vcard.BirthDay = bdayDes!.Read(rawBday!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawLogo, out var logoDes))
            vcard.Logo = logoDes!.Read(rawLogo!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(AgentFieldDeserializer.FieldKey,
                out var rawAgent, out var agentDes))
            vcard.Agent = agentDes!.Read(rawAgent!);

        if (vcardContent.TryGetDeserializationParams<IV2FieldDeserializer<string>>(MailerFieldDeserializer.FieldKey,
                out var rawMailer, out var mailerDes))
            vcard.Mailer = mailerDes!.Read(rawMailer!);

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<string>>(
                CategoriesFieldDeserializer.FieldKey,
                out var rawCategories, out var categoriesDes))
            foreach (var rawCategory in rawCategories)
                vcard.Categories.Add(categoriesDes!.Read(rawCategory));

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<string>>(
                MemberFieldDeserializer.FieldKey,
                out var rawMembers, out var membersDes))
            foreach (var rawMember in rawMembers)
                vcard.Members.Add(membersDes!.Read(rawMember));

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<TelephoneNumber>>(
                TelephoneNumberFieldDeserializer.FieldKey,
                out var rawTels, out var telDes))
            foreach (var rawTel in rawTels)
                vcard.PhoneNumbers.Add(telDes!.Read(rawTel));

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<EmailAddress>>(
                EmailAddressFieldDeserializer.FieldKey,
                out var rawEmails, out var emailDes))
            foreach (var rawEmail in rawEmails)
                vcard.EmailAddresses.Add(emailDes!.Read(rawEmail));

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawPhotos, out var photoDes))
            foreach (var rawPhoto in rawPhotos)
                vcard.Photos.Add(photoDes!.Read(rawPhoto));

        if (vcardContent.TryGetGroupDeserializationParams<IV2FieldDeserializer<Address>>(
                AddressFieldDeserializer.FieldKey,
                out var rawAddrs, out var addrDes))
            foreach (var rawAddr in rawAddrs)
                vcard.Addresses.Add(addrDes!.Read(rawAddr));

        if (vcardContent.TryGetCustomDeserializationParams<IV2FieldDeserializer<KeyValuePair<string, string>>>(
                out var rawCustoms, out var customDes))
            foreach (var rawCustom in rawCustoms)
                vcard.CustomFields.Add(customDes!.Read(rawCustom));

        return vcard;
    }

    private static vCard DeserializeV3(IReadOnlyCollection<string> vcardContent)
    {
        var vcard = new vCard(vCardVersion.v3);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Name>>(NameFieldDeserializer.FieldKey,
                out var rawName, out var nameDes))
            vcard.Name = nameDes!.Read(rawName!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(FormattedNameDeserializer.FieldKey,
                out var rawFormattedName, out var fnDes))
            vcard.FormattedName = fnDes!.Read(rawFormattedName!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(NicknameFieldDeserializer.FieldKey,
                out var rawNick, out var nickDes))
            vcard.NickName = nickDes!.Read(rawNick!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(NoteFieldDeserializer.FieldKey,
                out var rawNote, out var noteDes))
            vcard.Note = noteDes!.Read(rawNote!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(UrlFieldDeserializer.FieldKey,
                out var rawUrl, out var urlDes))
            vcard.Url = urlDes!.Read(rawUrl!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(TimezoneFieldDeserializer.FieldKey,
                out var rawTz, out var tzDes))
            vcard.Timezone = tzDes!.Read(rawTz!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Geo>>(GeoFieldDeserializer.FieldKey,
                out var rawGeo, out var geoDes))
            vcard.Geo = geoDes!.Read(rawGeo!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Organization>>(
                OrganizationFieldDeserializer.FieldKey,
                out var rawOrg, out var orgDes))
            vcard.Organization = orgDes!.Read(rawOrg!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(TitleFieldDeserializer.FieldKey,
                out var rawTitle, out var titleDes))
            vcard.Title = titleDes!.Read(rawTitle!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<ContactKind>>(KindFieldDeserializer.FieldKey,
                out var rawKind, out var kindDes))
            vcard.Kind = kindDes!.Read(rawKind!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Gender>>(GenderFieldDeserializer.FieldKey,
                out var rawGender, out var genderDes))
            vcard.Gender = genderDes!.Read(rawGender!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<DateTime>>(RevisionFieldDeserializer.FieldKey,
                out var rawRev, out var revDes))
            vcard.Revision = revDes!.Read(rawRev!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Language>>(LanguageFieldDeserializer.FieldKey,
                out var rawLang, out var langDes))
            vcard.Language = langDes!.Read(rawLang!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<DateTime>>(BirthdayFieldDeserializer.FieldKey,
                out var rawBday, out var bdayDes))
            vcard.BirthDay = bdayDes!.Read(rawBday!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawLogo, out var logoDes))
            vcard.Logo = logoDes!.Read(rawLogo!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(AgentFieldDeserializer.FieldKey,
                out var rawAgent, out var agentDes))
            vcard.Agent = agentDes!.Read(rawAgent!);

        if (vcardContent.TryGetDeserializationParams<IV3FieldDeserializer<string>>(MailerFieldDeserializer.FieldKey,
                out var rawMailer, out var mailerDes))
            vcard.Mailer = mailerDes!.Read(rawMailer!);

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<string>>(
                CategoriesFieldDeserializer.FieldKey,
                out var rawCategories, out var categoriesDes))
            foreach (var rawCategory in rawCategories)
                vcard.Categories.Add(categoriesDes!.Read(rawCategory));

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<string>>(
                MemberFieldDeserializer.FieldKey,
                out var rawMembers, out var membersDes))
            foreach (var rawMember in rawMembers)
                vcard.Members.Add(membersDes!.Read(rawMember));

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<TelephoneNumber>>(
                TelephoneNumberFieldDeserializer.FieldKey,
                out var rawTels, out var telDes))
            foreach (var rawTel in rawTels)
                vcard.PhoneNumbers.Add(telDes!.Read(rawTel));

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<EmailAddress>>(
                EmailAddressFieldDeserializer.FieldKey,
                out var rawEmails, out var emailDes))
            foreach (var rawEmail in rawEmails)
                vcard.EmailAddresses.Add(emailDes!.Read(rawEmail));

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawPhotos, out var photoDes))
            foreach (var rawPhoto in rawPhotos)
                vcard.Photos.Add(photoDes!.Read(rawPhoto));

        if (vcardContent.TryGetGroupDeserializationParams<IV3FieldDeserializer<Address>>(
                AddressFieldDeserializer.FieldKey,
                out var rawAddrs, out var addrDes))
            foreach (var rawAddr in rawAddrs)
                vcard.Addresses.Add(addrDes!.Read(rawAddr));

        if (vcardContent.TryGetCustomDeserializationParams<IV3FieldDeserializer<KeyValuePair<string, string>>>(
                out var rawCustoms, out var customDes))
            foreach (var rawCustom in rawCustoms)
                vcard.CustomFields.Add(customDes!.Read(rawCustom));

        return vcard;
    }

    private static vCard DeserializeV4(IReadOnlyCollection<string> vcardContent)
    {
        var vcard = new vCard(vCardVersion.v4);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Name>>(NameFieldDeserializer.FieldKey,
                out var rawName, out var nameDes))
            vcard.Name = nameDes!.Read(rawName!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(FormattedNameDeserializer.FieldKey,
                out var rawFormattedName, out var fnDes))
            vcard.FormattedName = fnDes!.Read(rawFormattedName!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(NicknameFieldDeserializer.FieldKey,
                out var rawNick, out var nickDes))
            vcard.NickName = nickDes!.Read(rawNick!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(NoteFieldDeserializer.FieldKey,
                out var rawNote, out var noteDes))
            vcard.Note = noteDes!.Read(rawNote!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(UrlFieldDeserializer.FieldKey,
                out var rawUrl, out var urlDes))
            vcard.Url = urlDes!.Read(rawUrl!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(TimezoneFieldDeserializer.FieldKey,
                out var rawTz, out var tzDes))
            vcard.Timezone = tzDes!.Read(rawTz!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Geo>>(GeoFieldDeserializer.FieldKey,
                out var rawGeo, out var geoDes))
            vcard.Geo = geoDes!.Read(rawGeo!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Organization>>(
                OrganizationFieldDeserializer.FieldKey,
                out var rawOrg, out var orgDes))
            vcard.Organization = orgDes!.Read(rawOrg!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(TitleFieldDeserializer.FieldKey,
                out var rawTitle, out var titleDes))
            vcard.Title = titleDes!.Read(rawTitle!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<ContactKind>>(KindFieldDeserializer.FieldKey,
                out var rawKind, out var kindDes))
            vcard.Kind = kindDes!.Read(rawKind!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Gender>>(GenderFieldDeserializer.FieldKey,
                out var rawGender, out var genderDes))
            vcard.Gender = genderDes!.Read(rawGender!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<DateTime>>(RevisionFieldDeserializer.FieldKey,
                out var rawRev, out var revDes))
            vcard.Revision = revDes!.Read(rawRev!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Language>>(LanguageFieldDeserializer.FieldKey,
                out var rawLang, out var langDes))
            vcard.Language = langDes!.Read(rawLang!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<DateTime>>(BirthdayFieldDeserializer.FieldKey,
                out var rawBday, out var bdayDes))
            vcard.BirthDay = bdayDes!.Read(rawBday!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawLogo, out var logoDes))
            vcard.Logo = logoDes!.Read(rawLogo!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(AgentFieldDeserializer.FieldKey,
                out var rawAgent, out var agentDes))
            vcard.Agent = agentDes!.Read(rawAgent!);

        if (vcardContent.TryGetDeserializationParams<IV4FieldDeserializer<string>>(MailerFieldDeserializer.FieldKey,
                out var rawMailer, out var mailerDes))
            vcard.Mailer = mailerDes!.Read(rawMailer!);

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<string>>(
                CategoriesFieldDeserializer.FieldKey,
                out var rawCategories, out var categoriesDes))
            foreach (var rawCategory in rawCategories)
                vcard.Categories.Add(categoriesDes!.Read(rawCategory));

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<string>>(
                MemberFieldDeserializer.FieldKey,
                out var rawMembers, out var membersDes))
            foreach (var rawMember in rawMembers)
                vcard.Members.Add(membersDes!.Read(rawMember));

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<TelephoneNumber>>(
                TelephoneNumberFieldDeserializer.FieldKey,
                out var rawTels, out var telDes))
            foreach (var rawTel in rawTels)
                vcard.PhoneNumbers.Add(telDes!.Read(rawTel));

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<EmailAddress>>(
                EmailAddressFieldDeserializer.FieldKey,
                out var rawEmails, out var emailDes))
            foreach (var rawEmail in rawEmails)
                vcard.EmailAddresses.Add(emailDes!.Read(rawEmail));

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<Photo>>(PhotoFieldDeserializer.FieldKey,
                out var rawPhotos, out var photoDes))
            foreach (var rawPhoto in rawPhotos)
                vcard.Photos.Add(photoDes!.Read(rawPhoto));

        if (vcardContent.TryGetGroupDeserializationParams<IV4FieldDeserializer<Address>>(
                AddressFieldDeserializer.FieldKey,
                out var rawAddrs, out var addrDes))
            foreach (var rawAddr in rawAddrs)
                vcard.Addresses.Add(addrDes!.Read(rawAddr));

        if (vcardContent.TryGetCustomDeserializationParams<IV4FieldDeserializer<KeyValuePair<string, string>>>(
                out var rawCustoms, out var customDes))
            foreach (var rawCustom in rawCustoms)
                vcard.CustomFields.Add(customDes!.Read(rawCustom));

        return vcard;
    }

    private static bool TryGetDeserializationParams<T>(this IEnumerable<string> vcardContent, string fieldKey,
        out string? rawData, out T? deserializer) where T : IFieldDeserializer
    {
        rawData = vcardContent.FirstOrDefault(x => x.StartsWith(fieldKey));

        if (rawData == null)
        {
            deserializer = default;
            return false;
        }

        deserializer = (T)FieldDeserializers[fieldKey];
        return true;
    }

    private static bool TryGetGroupDeserializationParams<T>(this IEnumerable<string> vcardContent, string fieldKey,
        out IEnumerable<string> rawData, out T? deserializer) where T : IFieldDeserializer
    {
        rawData = vcardContent.Where(x => x.StartsWith(fieldKey)).ToList();

        if (!rawData.Any())
        {
            deserializer = default;
            return false;
        }

        deserializer = (T)FieldDeserializers[fieldKey];
        return true;
    }

    private static bool TryGetCustomDeserializationParams<T>(this IEnumerable<string> vcardContent,
        out IEnumerable<string> rawData, out T? deserializer) where T : IFieldDeserializer
    {
        var knownFieldKeys = FieldDeserializers.Keys.Except(new[] { UnknownFieldDeserializer.Key });
        rawData = vcardContent.Where(x => !knownFieldKeys.All(x.StartsWith)).ToList();

        if (!rawData.Any())
        {
            deserializer = default;
            return false;
        }

        deserializer = (T)FieldDeserializers[UnknownFieldDeserializer.Key];
        return true;
    }

    #endregion
}
