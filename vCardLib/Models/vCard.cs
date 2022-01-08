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
    /// The Family name or Surname of the contact
    /// </summary>
    public string FamilyName { get; set; }

    /// <summary>
    /// The Firstname or Given name of the contact
    /// </summary>
    public string GivenName { get; set; }

    /// <summary>
    /// Any additional name of the contact
    /// </summary>
    public string AdditionalNames { get; set; }

    /// <summary>
    /// The honorific prefix of the contact
    /// </summary>
    public string HonorificPrefix { get; set; }

    /// <summary>
    /// The honorific suffix of the contact
    /// </summary>
    public string HonorificSuffix { get; set; }

    /// <summary>
    /// The full name of the contact
    /// </summary>
    public string FormattedName { get; set; }

    /// <summary>
    /// The contact's nickname
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// supplemental information or a comment that is associated with the vCard
    /// </summary>
    public string Note { get; set; }

    /// <summary>
    /// A url associated with the contact
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
    /// An organization the cotact belongs to
    /// </summary>
    public string Organization { get; set; }

    /// <summary>
    /// The contact's title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The contact type
    /// </summary>
    public ContactType Kind { get; set; }

    /// <summary>
    /// The contact's gender
    /// </summary>
    public GenderType Gender { get; set; }

    /// <summary>
    /// The contact's language
    /// </summary>
    /// <example>
    /// vcard.Language = "en-US";
    /// </example>
    public string Language { get; set; }

    /// <summary>
    /// The contact's birthday
    /// </summary>
    public DateTime? BirthDay { get; set; }

    /// <summary>
    /// The contact's birth place
    /// </summary>
    public string BirthPlace { get; set; }

    /// <summary>
    /// The contact's death place
    /// </summary>
    public string DeathPlace { get; set; }

    /// <summary>
    /// The last time the vCard was updated
    /// </summary>
    public DateTime? Revision { get; set; }

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
    /// The contact's areas of expertise
    /// </summary>
    public List<Expertise> Expertises { get; set; }

    /// <summary>
    /// The contact's hobbies
    /// </summary>
    public List<Hobby> Hobbies { get; set; }

    /// <summary>
    /// The contact's interests
    /// </summary>
    public List<Interest> Interests { get; set; }

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