using System;
using System.Collections.Generic;
using vCardLib.Enums;

namespace vCardLib.Models
{
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
        /// The middle name of the contact
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The prefix of the contact
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// The suffix of the contact
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// The full name of the contact
        /// </summary>
        public string FormattedName { get; set; }

        /// <summary>
        /// The contact's nickname
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// The contants special note
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// A url associated with the contact
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The contact's timezone
        /// </summary>
        /// <example>
        /// vcard.TimeZone = "GMT-1";
        /// </example>
        public string TimeZone { get; set; }

        /// <summary>
        /// The contacts geographical location specified by a longitude and latitude
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
        public List<PhoneNumber> PhoneNumbers { get; set; }

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
        /// The contact'c areas of expertise
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
            PhoneNumbers = new List<PhoneNumber>();
            EmailAddresses = new List<EmailAddress>();
            Pictures = new List<Photo>();
            Addresses = new List<Address>();
            Interests = new List<Interest>();
            Hobbies = new List<Hobby>();
            Expertises = new List<Expertise>();
            CustomFields = new List<KeyValuePair<string, string>>();
            Revision = DateTime.UtcNow;
        }
    }
}
