using System;
using System.Collections.Generic;
using vCardLib.Enums;
using vCardLib.Serializers;

namespace vCardLib.Models;

/// <summary>
/// Class to store the various vCard contact details
/// </summary>
// ReSharper disable once InconsistentNaming
public class vCard
{
    /// <summary>
    /// The version of the vcf file
    /// </summary>
    public vCardVersion Version { get; set; }

    /// <summary>
    /// components of the name of the object the vCard represents.
    /// </summary>
    public Name Name { get; set; }

    /// <summary>
    /// formatted text corresponding to the name of the object the vCard represents.
    /// </summary>
    public string FormattedName { get; set; }

    /// <summary>
    /// text corresponding to the nickname of the object the vCard represents.
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// supplemental information or a comment that is associated with the vCard
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// uniform resource locator associated with the object to which the vCard refers.  Examples for individuals
    /// include personal web sites, blogs, and social networking site identifiers.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// information related to the time zone of the object the vCard represents
    /// </summary>
    /// <example>
    /// vcard.Timezone = "GMT-1";
    /// </example>
    public string Timezone { get; set; }

    /// <summary>
    /// information related to the global positioning of the object the vCard represents
    /// </summary>
    public Geo Geo { get; set; }

    /// <summary>
    /// organizational name and units associated with the vCard.
    /// </summary>
    public Organization Organization { get; set; }

    /// <summary>
    /// The contact's title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// the kind of object the vCard represents.
    /// </summary>
    public ContactKind? Kind { get; set; }

    /// <summary>
    /// the components of the sex and gender identity of the object the vCard represents.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// revision information about the current vCard.
    /// </summary>
    public DateTime? Revision { get; set; }

    /// <summary>
    /// the language(s) that may be used for contacting the entity associated with the vCard.
    /// </summary>
    /// <example>
    /// vcard.Language = "en-US";
    /// </example>
    public string Language { get; set; }

    /// <summary>
    /// the birth date of the object the vCard represents.
    /// </summary>
    public DateTime? BirthDay { get; set; }

    /// <summary>
    /// a member in the group this vCard represents.
    /// </summary>
    public List<string> Members { get; set; }

    /// <summary>
    /// A collection of phone numbers associated with the contact
    /// </summary>
    public List<TelephoneNumber> PhoneNumbers { get; set; }

    /// <summary>
    /// A collection of email addresses associated with the contact
    /// </summary>
    public List<EmailAddress> EmailAddresses { get; set; }

    /// <summary>
    /// A collection of photos associated with the contact
    /// </summary>
    public List<Photo> Pictures { get; set; }

    /// <summary>
    /// The contact's addresses
    /// </summary>
    public List<Address> Addresses { get; set; }

    /// <summary>
    /// All other fields not defined in the spec
    /// </summary>
    public List<KeyValuePair<string, string>> CustomFields { get; set; }

    /// <summary>
    /// Default constructor, it initializes the collections in the vCard object
    /// </summary>
    public vCard()
    {
        PhoneNumbers = new List<TelephoneNumber>();
        EmailAddresses = new List<EmailAddress>();
        Pictures = new List<Photo>();
        Addresses = new List<Address>();
        Interests = new List<Interest>();
        Hobbies = new List<Hobby>();
        Expertises = new List<Expertise>();
        CustomFields = new List<KeyValuePair<string, string>>();
        Revision = DateTime.UtcNow;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Serializer.Serialize(this);
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <param name="version">vCard version</param>
    /// <returns>A string that represents the current object.</returns>
    public string ToString(vCardVersion version)
    {
        var clone = this;
        clone.Version = version;
        return Serializer.Serialize(this);
    }
}