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
        public string FullName { get; set; }

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
        /// Default constructor, it initializes the collections in the vCard object
        /// </summary>
        public vCard()
        {
            PhoneNumbers = new PhoneNumberCollection();
            EmailAddresses = new EmailAddressCollection();
            Pictures = new PhotoCollection();
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
                vCardCollection contacts = new vCardCollection();
                string vcfString;
                using (StreamReader streamReader = new StreamReader(filepath))
                {
                    vcfString = streamReader.ReadToEnd();
                }
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
                                vcard.Version = Single.Parse(contactDetail.Replace("VERSION:", "").Trim());
                            }
                            else if (contactDetail.StartsWith("FN:"))
                            {
                                vcard.FullName = contactDetail.Replace("FN:", "").Trim();
                            }
                            else if (contactDetail.StartsWith("URL:"))
                            {
                                vcard.URL = contactDetail.Replace("URL:", "").Trim();
                            }
                            else if (contactDetail.StartsWith("ORG:"))
                            {
                                vcard.Organization = contactDetail.Replace("ORG:", "").Trim();
                            }
                            else if (contactDetail.StartsWith("TITLE:"))
                            {
                                vcard.Title = contactDetail.Replace("TITLE:", "").Trim();
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
                                    PhoneNumber emailAddress = new PhoneNumber();
                                    emailAddress.Number = phoneString;
                                    emailAddress.Type = PhoneNumberType.Cell;
                                    vcard.PhoneNumbers.Add(emailAddress);
                                }
                                else if (phoneString.StartsWith("HOME"))
                                {
                                    phoneString = phoneString.Replace(";VOICE", "");
                                    phoneString = phoneString.Replace("HOME:", "");
                                    PhoneNumber emailAddress = new PhoneNumber();
                                    emailAddress.Number = phoneString;
                                    emailAddress.Type = PhoneNumberType.Home;
                                    vcard.PhoneNumbers.Add(emailAddress);
                                }
                                else if (phoneString.StartsWith("WORK"))
                                {
                                    phoneString = phoneString.Replace(";VOICE", "");
                                    phoneString = phoneString.Replace("WORK:", "");
                                    PhoneNumber emailAddress = new PhoneNumber();
                                    emailAddress.Number = phoneString;
                                    emailAddress.Type = PhoneNumberType.Work;
                                    vcard.PhoneNumbers.Add(emailAddress);
                                }
                                else if (phoneString.StartsWith("VOICE"))
                                {
                                    phoneString = phoneString.Replace("VOICE:", "");
                                    PhoneNumber emailAddress = new PhoneNumber();
                                    emailAddress.Number = phoneString;
                                    emailAddress.Type = PhoneNumberType.Voice;
                                    vcard.PhoneNumbers.Add(emailAddress);
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
                                    emailAddress.Type = EmailType.Cell;
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
                                string photoString = contactDetail + "\r\n";
                                while (true)
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
                                photoString = photoString.Replace("PHOTO;ENCODING=BASE64;JPEG:", "");

                                byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(photoString);
                                string photoString64 = System.Convert.ToBase64String(bytes);

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
                        contacts.Add(vcard);
                    }
                    return contacts;
                }
            }
        }
    }
}
