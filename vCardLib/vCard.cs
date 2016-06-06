
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
            vCardCollection vcardCollection = new vCardCollection();
            //TODO: implement card getting logic
            return vcardCollection;
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
