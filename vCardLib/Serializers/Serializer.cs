using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Serializers;

/// <summary>
/// Class holding shared logic
/// </summary>
public abstract class Serializer
{
    private static void WriteCardToBuilder(StringBuilder stringBuilder, vCard vCard)
    {
        Serializer serializer;

        switch (vCard.Version)
        {
            case vCardVersion.V2:
                serializer = new v2Serializer();
                break;
            case vCardVersion.V3:
                serializer = new v3Serializer();
                break;
            case vCardVersion.V4:
                serializer = new v4Serializer();
                break;
            default:
                throw new InvalidOperationException();
        }

        // add fields in order
        serializer.AddCardStart(stringBuilder);
        serializer.AddVersion(stringBuilder);
        serializer.AddRevision(stringBuilder, vCard.Revision);
        serializer.AddName(stringBuilder, vCard.FamilyName, vCard.GivenName, vCard.MiddleName, vCard.Prefix,
            vCard.Suffix);
        serializer.AddFormattedName(stringBuilder, vCard.FormattedName);
        serializer.AddOrganization(stringBuilder, vCard.Organization);
        serializer.AddTitle(stringBuilder, vCard.Title);
        serializer.AddUrl(stringBuilder, vCard.Url);
        serializer.AddNickName(stringBuilder, vCard.NickName);
        serializer.AddLanguage(stringBuilder, vCard.Language);
        serializer.AddBirthPlace(stringBuilder, vCard.BirthPlace);
        serializer.AddDeathPlace(stringBuilder, vCard.DeathPlace);
        serializer.AddTimeZone(stringBuilder, vCard.TimeZone);
        serializer.AddNote(stringBuilder, vCard.Note);
        serializer.AddContactKind(stringBuilder, vCard.Kind);
        serializer.AddGender(stringBuilder, vCard.Gender);
        serializer.AddGeo(stringBuilder, vCard.Geo);
        serializer.AddBirthday(stringBuilder, vCard.BirthDay);
        serializer.AddPhoneNumbers(stringBuilder, vCard.PhoneNumbers);
        serializer.AddEmailAddresses(stringBuilder, vCard.EmailAddresses);
        serializer.AddAddresses(stringBuilder, vCard.Addresses);
        serializer.AddPhotos(stringBuilder, vCard.Pictures);
        serializer.AddExpertises(stringBuilder, vCard.Expertises);
        serializer.AddHobbies(stringBuilder, vCard.Hobbies);
        serializer.AddInterests(stringBuilder, vCard.Interests);
        serializer.AddCardEnd(stringBuilder);
    }

    public static string Serialize(vCard vCard)
    {
        var stringBuilder = new StringBuilder();
        WriteCardToBuilder(stringBuilder, vCard);
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Serialize a vCard to a UTF-8 string and write to a stream
    /// </summary>
    /// <param name="vCard">vCard to be written</param>
    /// <param name="stream">Stream to be written to</param>
    public static async Task SerializeToStream(vCard vCard, Stream stream)
    {
        var content = Serialize(vCard);
        using var streamWriter = new StreamWriter(stream, default, -1, true);
        await streamWriter.WriteAsync(content);
    }

    public static string Serialize(IEnumerable<vCard> vCardCollection)
    {
        var vcards = vCardCollection.ToArray();
        
        if (!vcards.Any())
            return string.Empty;

        var stringBuilder = new StringBuilder();
        foreach (var vCard in vcards)
            WriteCardToBuilder(stringBuilder, vCard);

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Serialize a vCard collection to a UTF-8 string and write to a stream
    /// </summary>
    /// <param name="vCardCollection">A collection of vCards</param>
    /// <param name="stream">Stream to be written to</param>
    public static async Task SerializeToStream(IEnumerable<vCard> vCardCollection, Stream stream)
    {
        var content = Serialize(vCardCollection);
        using var streamWriter = new StreamWriter(stream, default, -1, true);
        await streamWriter.WriteAsync(content);
    }

    protected void AddCardStart(StringBuilder stringBuilder)
    {
        if (stringBuilder.Length > 0)
            stringBuilder.AppendLine();

        stringBuilder.AppendLine(FieldKeyConstants.StartToken);
    }

    protected void AddCardEnd(StringBuilder stringBuilder)
    {
        stringBuilder
            .Append(FieldKeyConstants.EndToken);
    }

    protected void AddRevision(StringBuilder stringBuilder, DateTime? revision)
    {
        stringBuilder.AppendLine($"REV:{(revision ?? DateTime.Now):yyyyMMddTHHmmssZ}");
    }

    protected void AddName(StringBuilder stringBuilder, string familyName, string givenName, string middleName,
        string prefix, string suffix)
    {
        stringBuilder.AppendLine(
            $"N:{familyName};{givenName};{middleName};{prefix};{suffix}"
        );
    }

    protected void AddFormattedName(StringBuilder stringBuilder, string formattedName)
    {
        if (!string.IsNullOrWhiteSpace(formattedName)) stringBuilder.AppendLine($"FN:{formattedName}");
    }

    protected void AddOrganization(StringBuilder stringBuilder, string organization)
    {
        if (!string.IsNullOrWhiteSpace(organization)) stringBuilder.AppendLine($"ORG:{organization}");
    }

    protected void AddTitle(StringBuilder stringBuilder, string title)
    {
        if (!string.IsNullOrWhiteSpace(title)) stringBuilder.AppendLine($"TITLE:{title}");
    }

    protected void AddUrl(StringBuilder stringBuilder, string url)
    {
        if (!string.IsNullOrWhiteSpace(url)) stringBuilder.AppendLine($"URL:{url}");
    }

    protected void AddNickName(StringBuilder stringBuilder, string nickName)
    {
        if (!string.IsNullOrWhiteSpace(nickName)) stringBuilder.AppendLine($"NICKNAME:{nickName}");
    }

    protected void AddLanguage(StringBuilder stringBuilder, string language)
    {
        if (!string.IsNullOrWhiteSpace(language)) stringBuilder.AppendLine($"LANG:{language}");
    }

    protected void AddBirthPlace(StringBuilder stringBuilder, string birthPlace)
    {
        if (!string.IsNullOrWhiteSpace(birthPlace)) stringBuilder.AppendLine($"BIRTHPLACE:{birthPlace}");
    }

    protected void AddDeathPlace(StringBuilder stringBuilder, string deathPlace)
    {
        if (!string.IsNullOrWhiteSpace(deathPlace)) stringBuilder.AppendLine($"DEATHPLACE:{deathPlace}");
    }

    protected void AddTimeZone(StringBuilder stringBuilder, string timeZone)
    {
        if (!string.IsNullOrWhiteSpace(timeZone)) stringBuilder.AppendLine($"TZ:{timeZone}");
    }

    protected void AddNote(StringBuilder stringBuilder, string note)
    {
        if (!string.IsNullOrWhiteSpace(note)) stringBuilder.AppendLine($"NOTE:{note}");
    }

    protected void AddContactKind(StringBuilder stringBuilder, ContactType contactType)
    {
        stringBuilder.AppendLine($"KIND:{contactType.ToString()}");
    }

    protected void AddGender(StringBuilder stringBuilder, GenderType genderType)
    {
        stringBuilder.AppendLine($"GENDER:{genderType.ToString()}");
    }

    protected void AddGeo(StringBuilder stringBuilder, Geo geo)
    {
        if (geo != null)
            stringBuilder.AppendLine("GEO:" + geo.Longitude + ";" + geo.Latitude);
    }

    protected void AddBirthday(StringBuilder stringBuilder, DateTime? birthDay)
    {
        if (birthDay.HasValue)
            stringBuilder.AppendLine($"BDAY:{birthDay.Value:yyyy-MM-dd}");
    }

    protected abstract void AddVersion(StringBuilder stringBuilder);

    protected abstract void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<TelephoneNumber> phoneNumbers);

    protected abstract void AddEmailAddresses(StringBuilder stringBuilder,
        IEnumerable<EmailAddress> emailAddresses);

    protected abstract void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses);

    protected abstract void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos);

    protected abstract void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises);

    protected abstract void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies);

    protected abstract void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests);
}