
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System;
using System.Drawing;
using System.IO;
using System.ComponentModel;

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

        public string NickName { get; set; }

        public ContactType Kind { get; set; }

        public GenderType Gender { get; set; }

        public AddressCollection Addresses { get; set; }

        public string Language { get; set; }

        public DateTime BirthDay { get; set; }

        public string BirthPlace { get; set; }

        public ExpertiseCollection Expertises { get; set; }

        public string DeathPlace { get; set; }

        public HobbyCollection Hobbies { get; set; }

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
        }

        /// <summary>
        /// Method to read in a vcf file, and extract all contact information into a vCardCollection
        /// </summary>
        /// <param name="filepath">Path to the vcf file</param>
        /// <returns>A collection of vCard objects, each with a contacts' full details</returns>
        public static vCardCollection FromFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
                throw new ArgumentNullException("The filepath supplied is null or empty");
            else if (!File.Exists(filepath))
                throw new FileNotFoundException("The specified file at the filepath does not exist");
            else
            {
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    return FromStreamReader(streamReader);
                }
            }
        }

        public static vCardCollection FromStreamReader(StreamReader stream)
        {
            if (stream == null)
                throw new NullReferenceException("The input stream cannot be null");
            else
            {
                vCardCollection contacts = new vCardCollection();
                string vcfString;
                vcfString = stream.ReadToEnd();
                if (!(vcfString.Contains("BEGIN:VCARD") && vcfString.Contains("END:VCARD")))
                    throw new InvalidOperationException("The vcf file does not seem to be a valid vcf file");
                else
                {
                    vcfString = vcfString.Replace("BEGIN:VCARD", "");
                    string[] contactStrings = vcfString.Split(new string[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string contactString in contactStrings)
                    {
                        vCard vcard = new vCard();
                        string[] contactDetails = contactString.Replace("PREF;", "").Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < contactDetails.Length; i++)
                        {
                            string contactDetail = contactDetails[i];
                            if (contactDetail.StartsWith("VERSION:"))
                            {
                                vcard.Version = float.Parse(contactDetail.Replace("VERSION:", "").Trim());
                            }
                            if (vcard.Version.Equals(2.1F))
                            {
                                if (contactDetail.StartsWith("PHOTO;"))
                                {
                                    string photoString = contactDetail + "\r\n";
                                    while (true)
                                    {
                                        if (i >= contactDetails.Length)
                                        {
                                            if (contactDetails[i + 1].StartsWith("PHOTO;"))
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                i++;
                                                photoString += contactDetails[i] + "\r\n";
                                            }
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    ProcessV2_1(ref vcard, contactDetail);
                                }
                                else
                                {
                                    ProcessV2_1(ref vcard, contactDetail);
                                }
                            }
                            else if (vcard.Version.Equals(3.0F))
                            {
                                ProcessV3_0(ref vcard, contactDetail);
                            }
                            else if (vcard.Version.Equals(4.0F))
                            {

                            }
                        }
                        contacts.Add(vcard);
                    }
                    return contacts;
                }
            }
        }

        private static void ProcessV2_1(ref vCard vcard, string contactDetail)
        {
            if (contactDetail.StartsWith("FN:"))
            {
                vcard.FormattedName = contactDetail.Replace("FN:", "").Trim();
            }
            else if (contactDetail.StartsWith("URL:"))
            {
                vcard.URL = contactDetail.Replace("URL:", "").Trim();
            }
            else if (contactDetail.StartsWith("GENDER:"))
            {
                string genderString = contactDetail.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male")
                    vcard.Gender = GenderType.Male;
                else if (genderString.ToLower() == "female")
                    vcard.Gender = GenderType.Female;
                else
                    vcard.Gender = GenderType.Other;
            }
            else if (contactDetail.StartsWith("ORG:"))
            {
                vcard.Organization = contactDetail.Replace("ORG:", "").Trim();
            }
            else if (contactDetail.StartsWith("TITLE:"))
            {
                vcard.Title = contactDetail.Replace("TITLE:", "").Trim();
            }
            else if (contactDetail.StartsWith("LANG:"))
            {
                vcard.Language = contactDetail.Replace("LANG:", "").Trim();
            }
            else if (contactDetail.StartsWith("NICKNAME:"))
            {
                vcard.NickName = contactDetail.Replace("NICKNAME:", "").Trim();
            }
            else if (contactDetail.StartsWith("BIRTHPLACE"))
            {
                vcard.BirthPlace = contactDetail.Replace("BIRTHPLACE;", "").Replace("TEXT:", "").Replace("URI:", "").Trim();
            }
            else if (contactDetail.StartsWith("DEATHPLACE"))
            {
                vcard.DeathPlace = contactDetail.Replace("DEATHPLACE;", "").Replace("TEXT:", "").Replace("URI:", "").Trim();
            }
            else if (contactDetail.StartsWith("BDAY"))
            {
                string dateString = contactDetail.Replace("BDAY:", "").Trim();
                if (dateString.Length == 8)
                    vcard.BirthDay = new DateTime(int.Parse(dateString.Substring(0, 4)), int.Parse(dateString.Substring(4, 2)), int.Parse(dateString.Substring(6, 2)));
            }
            else if (contactDetail.StartsWith("N:"))
            {
                string[] names = contactDetail.Replace("N:", "").Split(new string[] { ";" }, StringSplitOptions.None);
                if (names.Length > 0)
                    vcard.Firstname = names[0];
                if (names.Length > 1)
                    vcard.Surname = names[1];
                for (int j = 2; j < names.Length; j++)
                {
                    vcard.Othernames = names[j] + " ";
                }
            }
            else if (contactDetail.StartsWith("TEL;"))
            {
                string phoneString = contactDetail.Replace("TEL;", "");
                if (phoneString.StartsWith("CELL"))
                {
                    phoneString = phoneString.Replace(";VOICE", "");
                    phoneString = phoneString.Replace("CELL:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Cell;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("HOME"))
                {
                    phoneString = phoneString.Replace(";VOICE", "");
                    phoneString = phoneString.Replace("HOME:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Home;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("WORK"))
                {
                    phoneString = phoneString.Replace(";VOICE", "");
                    phoneString = phoneString.Replace("WORK:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Work;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VOICE:"))
                {
                    phoneString = phoneString.Replace("VOICE:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Voice;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("FAX"))
                {
                    phoneString = phoneString.Replace("FAX:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Fax;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXT"))
                {
                    phoneString = phoneString.Replace("TEXT:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Text;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VIDEO"))
                {
                    phoneString = phoneString.Replace("VIDEO:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Video;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("PAGER"))
                {
                    phoneString = phoneString.Replace("PAGER:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Pager;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXTPHONE"))
                {
                    phoneString = phoneString.Replace("TEXTPHONE:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Fax;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MAIN-NUMBER"))
                {
                    phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Fax;
                    vcard.PhoneNumbers.Add(phoneNumber);
                }
            }
            else if (contactDetail.StartsWith("EMAIL;"))
            {
                string emailString = contactDetail.Replace("EMAIL;", "");
                if (emailString.StartsWith("INTERNET:"))
                {
                    emailString = emailString.Replace("INTERNET:", "");
                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Email = new System.Net.Mail.MailAddress(emailString);
                    emailAddress.Type = EmailType.Internet;
                    vcard.EmailAddresses.Add(emailAddress);
                }
                else if (emailString.StartsWith("HOME:"))
                {
                    emailString = emailString.Replace("HOME:", "");
                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Email = new System.Net.Mail.MailAddress(emailString);
                    emailAddress.Type = EmailType.Home;
                    vcard.EmailAddresses.Add(emailAddress);
                }
                else if (emailString.StartsWith("WORK:"))
                {
                    emailString = emailString.Replace("WORK:", "");
                    EmailAddress emailAddress = new EmailAddress();
                    emailAddress.Email = new System.Net.Mail.MailAddress(emailString);
                    emailAddress.Type = EmailType.Work;
                    vcard.EmailAddresses.Add(emailAddress);
                }
            }
            /*else if (contactDetail.StartsWith("PHOTO;"))
            {
                contactDetail = contactDetail.Replace("PHOTO;ENCODING=BASE64;JPEG:", "");

                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(contactDetail);
                string photoString64 = Convert.ToBase64String(bytes);

                byte[] byteBuffer = Convert.FromBase64String(photoString64);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);
                memoryStream.Position = 0;
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap bitmap = (Bitmap)tc.ConvertFrom(byteBuffer);
                Photo photo = new Photo();
                photo.Picture = bitmap;

                memoryStream.Close();
                memoryStream = null;
                byteBuffer = null;

                vcard.Pictures.Add(photo);
            }*/
            else if (contactDetail.StartsWith("ADR;"))
            {
                string addressString = contactDetail.Replace("ADR;", "");
                if (addressString.StartsWith("HOME:"))
                {
                    addressString = addressString.Replace("HOME:", "");
                    Address address = new Address();
                    address.Location = addressString.Replace(";", " ");
                    address.Type = AddressType.Home;
                    vcard.Addresses.Add(address);
                }
                else if (addressString.StartsWith("WORK:"))
                {
                    addressString = addressString.Replace("WORK:", "");
                    Address address = new Address();
                    address.Location = addressString.Replace(";", " ");
                    address.Type = AddressType.Work;
                    vcard.Addresses.Add(address);
                }
            }
            else if (contactDetail.StartsWith("HOBBY"))
            {
                string hobbyString = contactDetail.Replace("HOBBY;", "");
                Hobby hobby = new Hobby();
                if (hobbyString.StartsWith("HIGH"))
                {
                    hobby.Level = Level.High;
                    hobby.Activity = hobbyString.Replace("HIGH:", "").Trim();
                    vcard.Hobbies.Add(hobby);
                }
                else if (hobbyString.StartsWith("MEDIUM"))
                {
                    hobby.Level = Level.Medium;
                    hobby.Activity = hobbyString.Replace("MEDIUM:", "").Trim();
                    vcard.Hobbies.Add(hobby);
                }
                else if (hobbyString.StartsWith("LOW"))
                {
                    hobby.Level = Level.Low;
                    hobby.Activity = hobbyString.Replace("LOW:", "").Trim();
                    vcard.Hobbies.Add(hobby);
                }
            }
            else if (contactDetail.StartsWith("INTEREST"))
            {
                string interestString = contactDetail.Replace("INTEREST;", "");
                Interest interest = new Interest();
                if (interestString.StartsWith("HIGH"))
                {
                    interest.Level = Level.High;
                    interest.Activity = interestString.Replace("HIGH:", "").Trim();
                    vcard.Interests.Add(interest);
                }
                else if (interestString.StartsWith("MEDIUM"))
                {
                    interest.Level = Level.Medium;
                    interest.Activity = interestString.Replace("MEDIUM:", "").Trim();
                    vcard.Interests.Add(interest);
                }
                else if (interestString.StartsWith("LOW"))
                {
                    interest.Level = Level.Low;
                    interest.Activity = interestString.Replace("LOW:", "").Trim();
                    vcard.Interests.Add(interest);
                }
            }
            else if (contactDetail.StartsWith("EXPERTISE"))
            {
                string expertiseString = contactDetail.Replace("EXPERTISE;", "");
                Expertise expertise = new Expertise();
                if (expertiseString.StartsWith("HIGH"))
                {
                    expertise.Level = Level.High;
                    expertise.Area = expertiseString.Replace("HIGH:", "").Trim();
                    vcard.Expertises.Add(expertise);
                }
                else if (expertiseString.StartsWith("MEDIUM"))
                {
                    expertise.Level = Level.Medium;
                    expertise.Area = expertiseString.Replace("MEDIUM:", "").Trim();
                    vcard.Expertises.Add(expertise);
                }
                else if (expertiseString.StartsWith("LOW"))
                {
                    expertise.Level = Level.Low;
                    expertise.Area = expertiseString.Replace("LOW:", "").Trim();
                    vcard.Expertises.Add(expertise);
                }
            }
            else if (contactDetail.StartsWith("KIND"))
            {
                string contactKindString = contactDetail.Replace("KIND:", "").Trim();
                if (contactKindString == "INDIVIDUAL")
                    vcard.Kind = ContactType.Individual;
                else if (contactKindString == "GROUP")
                    vcard.Kind = ContactType.Group;
                else if (contactKindString == "ORG")
                    vcard.Kind = ContactType.Organization;
                else if (contactKindString == "LOCATION")
                    vcard.Kind = ContactType.Location;
                else if (contactKindString == "APPLICATION")
                    vcard.Kind = ContactType.Application;
                else if (contactKindString == "DEVICE")
                    vcard.Kind = ContactType.Device;
            }
        }

        private static void ProcessV3_0(ref vCard vcard, string contactDetail)
        {

        }

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
