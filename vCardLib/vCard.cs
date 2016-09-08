
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System;
using System.IO;

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
        public float Version { get; set; }

        /// <summary>
        /// The lastname or Surname of the contact
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// The Firstname or Given name of the contact
        /// </summary>
        public string Firstname { get; set; }

        /// <summary>
        /// A concatenation of all other names the contact has
        /// </summary>
        public string Othernames { get; set; }

        /// <summary>
        /// The full name of the contact
        /// </summary>
        public string FormattedName { get; set; }

        /// <summary>
        /// A collection of phone numbers associated with the contact
        /// </summary>
        public PhoneNumberCollection PhoneNumbers { get; set; }

        /// <summary>
        /// A collection of email addresses associated with the contact
        /// </summary>
        public EmailAddressCollection EmailAddresses { get; set; }

        /// <summary>
        /// A url associated with the contact
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// An organization the cotact belongs to
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// The contact's title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A collection of photos associated with the contact
        /// </summary>
        public PhotoCollection Pictures { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ContactType Kind { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AddressCollection Addresses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ExpertiseCollection Expertises { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DeathPlace { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HobbyCollection Hobbies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public InterestCollection Interests { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TimeZone { get; set; }

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
        }

        /// <summary>
        /// Method to read in a vcf file, and extract all contact information into a vCardCollection
        /// </summary>
        /// <param name="filepath">Path to the vcf file</param>
        /// <returns>A collection of vCard objects, each with a contacts' full details</returns>
        public static vCardCollection FromFile(string filepath)
        {
            StreamReader sr = Helper.GetStreamReaderFromFile(filepath);
            return FromStreamReader(sr);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="streamReader"></param>
        /// <returns></returns>
        public static vCardCollection FromStreamReader(StreamReader streamReader)
        {
            vCardCollection collection = new vCardCollection();
            string contactsString = Helper.GetStringFromStreamReader(streamReader);
            string[] contacts = Helper.GetContactsArrayFromString(contactsString);
            foreach(string contact in contacts)
            {
                string[] contactDetails = Helper.GetContactDetailsArrayFromString(contact);
                if (contactDetails.Length > 0)
                {
                    vCard details = Helper.GetVcardFromDetails(contactDetails);
                    collection.Add(details);
                }
                else
                    continue;
            }
            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var toCompare = obj as vCard;
            if (toCompare == null)
                return false;
            return this.Firstname == toCompare.Firstname
                && this.Surname == toCompare.Surname
                && this.FormattedName == toCompare.FormattedName
                && this.PhoneNumbers.Count == toCompare.PhoneNumbers.Count
                && this.EmailAddresses.Count == toCompare.EmailAddresses.Count
                && this.BirthPlace == toCompare.BirthPlace
                && this.DeathPlace == toCompare.DeathPlace
                && this.Expertises.Count == toCompare.Expertises.Count;
        }
    }
}
