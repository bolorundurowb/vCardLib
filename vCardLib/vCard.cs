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
        /// The contact's nickname
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// The contact type
        /// </summary>
        public ContactType Kind { get; set; }

        /// <summary>
        /// The contact's gender
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// The contact's addresses
        /// </summary>
        public AddressCollection Addresses { get; set; }

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
        public DateTime BirthDay { get; set; }

        /// <summary>
        /// The contact's birth place
        /// </summary>
        public string BirthPlace { get; set; }

        /// <summary>
        /// The contact'c areas of expertise
        /// </summary>
        public ExpertiseCollection Expertises { get; set; }

        /// <summary>
        /// The contact's death place
        /// </summary>
        public string DeathPlace { get; set; }

        /// <summary>
        /// The contact's hobbies
        /// </summary>
        public HobbyCollection Hobbies { get; set; }

        /// <summary>
        /// The contact's interests
        /// </summary>
        public InterestCollection Interests { get; set; }

        /// <summary>
        /// The contact's timezone
        /// </summary>
        /// <example>
        /// vcard.TimeZone = "GMT-1";
        /// </example>
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
			//Set the default vCard version as 2.1
			Version = 2.1F;

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
            return Firstname == toCompare.Firstname
                && Surname == toCompare.Surname
                && FormattedName == toCompare.FormattedName
                && PhoneNumbers.Count == toCompare.PhoneNumbers.Count
                && EmailAddresses.Count == toCompare.EmailAddresses.Count
                && BirthPlace == toCompare.BirthPlace
                && DeathPlace == toCompare.DeathPlace
                && Expertises.Count == toCompare.Expertises.Count;
        }

        /// <summary>
        /// Save a vcard object to a vcf file
        /// </summary>
        /// <param name="filePath">Path to file to save to</param>
        /// <param name="writeOption">Option to determine if the method would overwrite the file or throw an error</param>
        /// <returns>A boolean value stating whether the save option was successful or not</returns>
        /// <exception cref="InvalidOperationException">The given file path already exists</exception>
        public bool Save(string filePath, WriteOptions writeOption = WriteOptions.ThrowError)
        {
            if(writeOption == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException("A file with the given filePath exists. If you want to overwrite the file, then call this method and pass the optional overwrite option");
                }
            }
            return Save(filePath, Version, writeOption);
        }

        /// <summary>
        /// Save a vcard object to a vcf file
        /// </summary>
        /// <param name="filePath">Path to file to save to</param>
        /// <param name="version">Set the save version</param>
        /// <param name="writeOption">Option to determine if the method would overwrite the file or throw an error</param>
        /// <returns>A boolean value stating whether the save option was successful or not</returns>
        /// <exception cref="InvalidOperationException">The file already exists</exception>
        public bool Save(string filePath, float version, WriteOptions writeOption = WriteOptions.ThrowError)
        {
            if (writeOption == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException("A file with the given filePath exists. If you want to overwrite the file, then call this method and pass the optional overwrite option");
                }
            }
            if (version == 2.1F)
            {
                string vcfString = "";
                WriteV2ObjectToString(ref vcfString);
                File.WriteAllText(filePath, vcfString);
            }
            else if (version == 3.0F)
            {
                string vcfString = "";
                WriteV3ObjectToString(ref vcfString);
                File.WriteAllText(filePath, vcfString);
            }
            else if (version == 4.0F)
            {
                throw new NotImplementedException("Writing for v4 is not implemented");
            }
            else
            {
                throw new ArgumentException("version is not a valid vcf version");
            }
            return true;
        }

        /// <summary>
        /// Write a vCard to a string
        /// </summary>
        /// <param name="vCardString">An empty string passed by reference</param>
        internal void WriteV3ObjectToString(ref string vCardString)
        {
            vCardString += "BEGIN:VCARD" + Environment.NewLine;
            vCardString += "VERSION:3.0" + Environment.NewLine;
            vCardString += "N:" + Firstname + ";" + Surname + ";" + Othernames + Environment.NewLine;
            vCardString += "FN:" + FormattedName + Environment.NewLine;
            vCardString += "ORG:" + Organization + Environment.NewLine;
            vCardString += "TITLE:" + Title + Environment.NewLine;
            vCardString += "URL:" + URL + Environment.NewLine;
            vCardString += "NICKNAME:" + NickName + Environment.NewLine;
            vCardString += "KIND:" + Kind.ToString().ToUpper() + Environment.NewLine;
            vCardString += "GENDER:" + Gender + Environment.NewLine;
            vCardString += "LANG:" + Language + Environment.NewLine;
            vCardString += "BIRTHPLACE:" + BirthPlace + Environment.NewLine;
            vCardString += "DEATHPLACE:" + DeathPlace + Environment.NewLine;
            vCardString += "TZ:" + TimeZone + Environment.NewLine;
			vCardString += "BDAY:" + BirthDay.Year + BirthDay.Month.ToString("00") + BirthDay.Day.ToString("00");
            foreach(PhoneNumber phoneNumber in PhoneNumbers)
            {
                vCardString += Environment.NewLine;
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    vCardString += "TEL:" + phoneNumber.Number;
                }
                else if (phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    vCardString += "TEL;TYPE=MAIN-NUMBER:" + phoneNumber.Number;
                }
                else
                {
                    vCardString += "TEL;TYPE=" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number;
                }
            }
            foreach(EmailAddress email in EmailAddresses)
            {
                vCardString += Environment.NewLine;
                if (email.Type == EmailType.None)
                {
                    vCardString += "EMAIL:" + email.Email.Address;
                }
                else
                {
                    vCardString += "EMAIL;TYPE=" + email.Type.ToString().ToUpper() + ":" + email.Email.Address;
                }
            }
            foreach(Address address in Addresses)
            {
                vCardString += Environment.NewLine;
                if (address.Type == AddressType.None)
                {
                    vCardString += "ADR:" + address.Location;
                }
                else
                {
                    vCardString += "ADR;TYPE=" + address.Type.ToString().ToUpper() + ":" + address.Location;
                }
            }
            foreach(Photo photo in Pictures)
            {
                vCardString += Environment.NewLine;
                vCardString += "PHOTO;TYPE=" + photo.Encoding;
                if(photo.Type == PhotoType.URL)
                {
                    vCardString += ";VALUE=URI:" + photo.PhotoURL;
                }
                else if(photo.Type == PhotoType.Image)
                {
                    vCardString += ";ENCODING=b:" + photo.ToBase64String();
                }
            }
            foreach(Expertise expertise in Expertises)
            {
                vCardString += Environment.NewLine;
                vCardString += "EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area;
            }
            foreach(Hobby hobby in Hobbies)
            {
                vCardString += Environment.NewLine;
                vCardString += "HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity;
            }
            foreach(Interest interest in Interests)
            {
                vCardString += Environment.NewLine;
                vCardString += "INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity;
            }
            vCardString += Environment.NewLine;
            vCardString += "END:VCARD";
        }

        /// <summary>
        /// Write a vCard to a string
        /// </summary>
        /// <param name="vCardString">An empty string passed by reference</param>
        internal void WriteV2ObjectToString(ref string vCardString)
        {
            vCardString += "BEGIN:VCARD" + Environment.NewLine;
            vCardString += "VERSION:2.1" + Environment.NewLine;
            vCardString += "N:" + Firstname + ";" + Surname + ";" + Othernames + Environment.NewLine;
            vCardString += "FN:" + FormattedName + Environment.NewLine;
            vCardString += "ORG:" + Organization + Environment.NewLine;
            vCardString += "TITLE:" + Title + Environment.NewLine;
            vCardString += "URL:" + URL + Environment.NewLine;
            vCardString += "NICKNAME:" + NickName + Environment.NewLine;
            vCardString += "KIND:" + Kind.ToString().ToUpper() + Environment.NewLine;
            vCardString += "GENDER:" + Gender + Environment.NewLine;
            vCardString += "LANG:" + Language + Environment.NewLine;
            vCardString += "BIRTHPLACE:" + BirthPlace + Environment.NewLine;
            vCardString += "DEATHPLACE:" + DeathPlace + Environment.NewLine;
            vCardString += "TZ:" + TimeZone + Environment.NewLine;
            vCardString += "BDAY:" + BirthDay.Year + BirthDay.ToString("00") + BirthDay.ToString("00");
            foreach (PhoneNumber phoneNumber in PhoneNumbers)
            {
                vCardString += Environment.NewLine;
                if (phoneNumber.Type == PhoneNumberType.None)
                {
                    vCardString += "TEL:" + phoneNumber.Number;
                }
                else if(phoneNumber.Type == PhoneNumberType.MainNumber)
                {
                    vCardString += "TEL;MAIN-NUMBER:" + phoneNumber.Number;
                }
                else
                {
                    vCardString += "TEL;" + phoneNumber.Type.ToString().ToUpper() + ":" + phoneNumber.Number;
                }
            }
            foreach (EmailAddress email in EmailAddresses)
            {
                vCardString += Environment.NewLine;
                if (email.Type == EmailType.None)
                {
                    vCardString += "EMAIL:" + email.Email.Address;
                }
                else
                {
                    vCardString += "EMAIL;" + email.Type.ToString().ToUpper() + ":" + email.Email.Address;
                }
            }
            foreach (Address address in Addresses)
            {
                vCardString += Environment.NewLine;
                if (address.Type == AddressType.None)
                {
                    vCardString += "ADR:" + address.Location;
                }
                else
                {
                    vCardString += "ADR;" + address.Type.ToString().ToUpper() + ":" + address.Location;
                }
            }
            foreach (Photo photo in Pictures)
            {
                vCardString += Environment.NewLine;
                vCardString += "PHOTO;" + photo.Encoding;
                if (photo.Type == PhotoType.URL)
                {
                    vCardString += ":" + photo.PhotoURL;
                }
                else if (photo.Type == PhotoType.Image)
                {
                    vCardString += ";ENCODING=BASE64:" + photo.ToBase64String();
                }
            }
            foreach (Expertise expertise in Expertises)
            {
                vCardString += Environment.NewLine;
                vCardString += "EXPERTISE;LEVEL=" + expertise.Level.ToString().ToLower() + ":" + expertise.Area;
            }
            foreach (Hobby hobby in Hobbies)
            {
                vCardString += Environment.NewLine;
                vCardString += "HOBBY;LEVEL=" + hobby.Level.ToString().ToLower() + ":" + hobby.Activity;
            }
            foreach (Interest interest in Interests)
            {
                vCardString += Environment.NewLine;
                vCardString += "INTEREST;LEVEL=" + interest.Level.ToString().ToLower() + ":" + interest.Activity;
            }
            vCardString += Environment.NewLine;
            vCardString += "END:VCARD";
        }
    }
}
