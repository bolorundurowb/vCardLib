using System;
using System.IO;
using vCardLib.Collections;
using vCardLib.Deserializers;
using vCardLib.Helpers;
using vCardLib.Models;
using vCardLib.Serializers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib
{
    /// <summary>
    /// Class to store the various vCard contact details
    /// </summary>
    public class vCard
    {
        /// <summary>
        /// The version of the vcf file
        /// </summary>
        public Version Version { get; set; }

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
        public DateTime Revision { get; set; }

        /// <summary>
        /// The contacts skype PSTN number
        /// </summary>
        public string XSkypePstnNumber { get; set; }

        /// <summary>
        /// The contacts skype display name
        /// </summary>
        public string XSkypeDisplayName { get; set; }

        /// <summary>
        /// A collection of phone numbers associated with the contact
        /// </summary>
        public PhoneNumberCollection PhoneNumbers { get; set; }

        /// <summary>
        /// A collection of email addresses associated with the contact
        /// </summary>
        public EmailAddressCollection EmailAddresses { get; set; }

        /// <summary>
        /// A collection of photos associated with the contact
        /// </summary>
        public PhotoCollection Pictures { get; set; }

        /// <summary>
        /// The contact's addresses
        /// </summary>
        public AddressCollection Addresses { get; set; }

        /// <summary>
        /// The contact'c areas of expertise
        /// </summary>
        public ExpertiseCollection Expertises { get; set; }

        /// <summary>
        /// The contact's hobbies
        /// </summary>
        public HobbyCollection Hobbies { get; set; }

        /// <summary>
        /// The contact's interests
        /// </summary>
        public InterestCollection Interests { get; set; }

        /// <summary>
        /// Default constructor, it initializes the collections in the vCard object
        /// </summary>
        public vCard()
        {
            PhoneNumbers = new PhoneNumberCollection();
            EmailAddresses = new EmailAddressCollection();
            Pictures = new PhotoCollection();
            Addresses = new AddressCollection();
            Interests = new InterestCollection();
            Hobbies = new HobbyCollection();
            Expertises = new ExpertiseCollection();
			//Set the default vCard version as 2.1
			Version = Version.V2;

        }

        /// <summary>
        /// Method to read in a vcf file, and extract all contact information into a vCardCollection
        /// </summary>
        /// <param name="filepath">Path to the vcf file</param>
        /// <returns>A collection of vCard objects, each with a contacts' full details</returns>
        [Obsolete("This method is now obsolete, use Deserializer.FromFile instead")]
        public static vCardCollection FromFile(string filepath)
        {
            return Deserializer.FromFile(filepath);
        }

        /// <summary>
        /// Method to read in a vcf file and extract all contents to a vCardColllection
        /// </summary>
        /// <param name="streamReader">A stream reader containing the vcard details</param>
        /// <returns>A collection of vCard objects, each with a contacts' full details</returns>
        [Obsolete("This method is now obsolete, use Deserializer.FromStreamReader instead")]
        public static vCardCollection FromStreamReader(StreamReader streamReader)
        {
            return Deserializer.FromStreamReader(streamReader);
        }

        /// <summary>
        /// Save a vcard object to a vcf file
        /// </summary>
        /// <param name="filePath">Path to file to save to</param>
        /// <param name="writeOption">Option to determine if the method would overwrite the file or throw an error</param>
        /// <returns>A boolean value stating whether the save option was successful or not</returns>
        public bool Save(string filePath, WriteOptions writeOption = WriteOptions.ThrowError)
        {
            return Save(filePath, Version, writeOption);
        }

        /// <summary>
        /// Save a vcard object to a vcf file
        /// </summary>
        /// <param name="filePath">Path to file to save to</param>
        /// <param name="version">Set the save version</param>
        /// <param name="writeOption">Option to determine if the method would overwrite the file or throw an error</param>
        /// <returns>A boolean value stating whether the save option was successful or not</returns>
        public bool Save(string filePath, Version version, WriteOptions writeOption = WriteOptions.ThrowError)
        {
            return Serializer.Serialize(this, filePath, version, writeOption);
        }
    }
}
