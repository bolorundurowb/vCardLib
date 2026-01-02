using System;
using System.Collections.Generic;
using vCardLib.Enums;

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
    public vCardVersion Version { get; private set; }

    /// <summary>
    /// Components of the name of the object the vCard represents.
    /// </summary>
    public Name? Name { get; set; }

    /// <summary>
    /// Formatted text corresponding to the name of the object the vCard represents.
    /// </summary>
    public string? FormattedName { get; set; }

    /// <summary>
    /// text corresponding to the nickname of the object the vCard represents.
    /// </summary>
    public string? NickName { get; set; }

    /// <summary>
    /// Supplemental information or a comment that is associated with the vCard
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Property is used to uniquely identify the object that the vCard represents
    /// </summary>
    public string? Uid { get; set; }

    /// <summary>
    /// Uniform resource locator associated with the object to which the vCard refers.  Examples for individuals
    /// include personal websites, blogs, and social networking site identifiers.
    /// </summary>
    public Url? Url { get; set; }

    /// <summary>
    /// Information related to the time zone of the object the vCard represents
    /// </summary>
    /// <example>
    /// vcard.Timezone = "GMT-1";
    /// </example>
    public string? Timezone { get; set; }

    /// <summary>
    /// Information related to the global positioning of the object the vCard represents
    /// </summary>
    public Geo? Geo { get; set; }

    /// <summary>
    /// Organizational name and units associated with the vCard.
    /// </summary>
    public Organization? Organization { get; set; }

    /// <summary>
    /// The contact's title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The kind of object the vCard represents.
    /// </summary>
    public ContactKind? Kind { get; set; }

    /// <summary>
    /// The components of the sex and gender identity of the object the vCard represents.
    /// </summary>
    public Gender? Gender { get; set; }

    /// <summary>
    /// Revision information about the current vCard.
    /// </summary>
    public DateTime? Revision { get; set; }

    /// <summary>
    /// The language(s) that may be used for contacting the entity associated with the vCard.
    /// </summary>
    /// <example>
    /// vcard.Language = "en-US";
    /// </example>
    public Language? Language { get; set; }

    /// <summary>
    /// The anniversary date of the object the vCard represents.
    /// </summary>
    public DateTime? Anniversary { get; set; }

    /// <summary>
    /// The birth date of the object the vCard represents.
    /// </summary>
    public DateTime? BirthDay { get; set; }

    /// <summary>
    /// The logo of the organization that is associated with the individual to which the vCard belongs
    /// </summary>
    public Photo? Logo { get; set; }

    /// <summary>
    /// Information about another person who will act on behalf of the vCard object.
    /// </summary>
    public string? Agent { get; set; }

    /// <summary>
    /// the type of electronic mail software that is used by the individual associated with the vCard.
    /// </summary>
    public string? Mailer { get; set; }

    /// <summary>
    /// category information about the vCard, also known as "tags"
    /// </summary>
    public List<string> Categories { get; set; } = new();

    /// <summary>
    /// a member in the group this vCard represents.
    /// </summary>
    public List<string> Members { get; set; } = new();

    /// <summary>
    /// A collection of phone numbers associated with the contact
    /// </summary>
    public List<TelephoneNumber> PhoneNumbers { get; set; } = new();

    /// <summary>
    /// A collection of email addresses associated with the contact
    /// </summary>
    public List<EmailAddress> EmailAddresses { get; set; } = new();

    /// <summary>
    /// A collection of photos associated with the contact
    /// </summary>
    public List<Photo> Photos { get; set; } = new();

    /// <summary>
    /// The contact's addresses
    /// </summary>
    public List<Address> Addresses { get; set; } = new();

    /// <summary>
    /// All other fields not defined in the spec
    /// </summary>
    public List<KeyValuePair<string, string>> CustomFields { get; set; } = new();

    public vCard(vCardVersion version) => Version = version;
}