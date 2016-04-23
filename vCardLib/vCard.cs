using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vCardLib
{
    public class vCard
    {
        public float Version { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Othernames { get; set; }
        public string FullName { get; set; }
        public PhoneNumberCollection PhoneNumbers { get; set; }
        public EmailAddressCollection EmailAddresses { get; set; }
        public string URL { get; set; }
        public string Organization { get; set; }
        public string Title { get; set; }
        public PhotoCollection Pictures { get; set; }

        public vCard()
        {
            PhoneNumbers = new PhoneNumberCollection();
            EmailAddresses = new EmailAddressCollection();
        }

        public static vCardCollection FromFile(string filepath)
        {
            vCardCollection contacts = new vCardCollection();
            string vcfString;
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                vcfString = streamReader.ReadToEnd();
            }
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
                        vcard.Firstname = names[0];
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
                   /* else if (contactDetail.StartsWith("PHOTO;"))
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

                        Bitmap picture = null;
                        byte[] byteBuffer = Convert.FromBase64String(photoString64);
                        MemoryStream memoryStream = new MemoryStream(byteBuffer);
                        memoryStream.Position = 0;
                        picture = (Bitmap)Bitmap.FromStream(memoryStream, false, true);
                        memoryStream.Close();
                        memoryStream = null;
                        byteBuffer = null;

                        Photo photo = new Photo();
                        photo.Picture = picture;

                        vcard.Pictures.Add(photo);
                    }*/
                }
                contacts.Add(vcard);
            }
            return contacts;
        }
    }
}
