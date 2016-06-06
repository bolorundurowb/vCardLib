using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace vCardLib
{
    public class Helper
    {
        public static StreamReader GetStreamReaderFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("The filepath supplied is null or empty");
            else if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file at the filepath does not exist");
            else
            {
                return new StreamReader(filePath);
            }
        }

        public static string GetStringFromStreamReader(StreamReader streamReader)
        {
            if (streamReader == null)
                throw new NullReferenceException("The input stream cannot be null");
            else
            {
                return streamReader.ReadToEnd();
            }
        }

        public static string[] GetContactsArrayFromString(string contactsString)
        {
            if (string.IsNullOrWhiteSpace(contactsString))
                throw new ArgumentException("string cannot be null, empty or composed of only whitespace characters");
            else if (!(contactsString.Contains("BEGIN:VCARD") && contactsString.Contains("END:VCARD")))
                throw new InvalidOperationException("The vcf file does not seem to be a valid vcf file");
            else
            {
                contactsString = contactsString.Replace("BEGIN:VCARD", "");
                return contactsString.Split(new string[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static string[] GetContactDetailsArrayFromString(string contactString)
        {
            contactString = contactString.Replace("PREF;", "");
            return contactString.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static vCard GetVcardFromDetails(string[] contactDetails)
        {
            string versionString = contactDetails.FirstOrDefault(s => s.StartsWith("VERSION:"));
            if (versionString == null)
                throw new InvalidOperationException("details do not contain a specification for 'Version'.");
            else
            {
                vCard vcard = new vCard();
                vcard.Version = float.Parse(versionString.Replace("VERSION:", "").Trim());
                if (vcard.Version.Equals(2.1F))
                    ProcessV2_1(ref vcard, contactDetails);
                else if (vcard.Version.Equals(3.0F))
                    ProcessV3_0(ref vcard);
                else if (vcard.Version.Equals(4.0F))
                    ProcessV4_0(ref vcard);
                return vcard;
            }
        }

        private static void ProcessV2_1(ref vCard vcard, string[] contactDetails)
        {
            #region Simple Properties
            string fnString = contactDetails.FirstOrDefault(s => s.StartsWith("FN:"));
            if (fnString != null)
                vcard.FormattedName = fnString.Replace("FN:", "").Trim();

            string titleString = contactDetails.FirstOrDefault(s => s.StartsWith("TITLE:"));
            if (titleString != null)
                vcard.Title = titleString.Replace("TITLE:", "").Trim();

            string urlString = contactDetails.FirstOrDefault(s => s.StartsWith("URL:"));
            if (urlString != null)
                vcard.URL = urlString.Replace("URL:", "").Trim();

            string orgString = contactDetails.FirstOrDefault(s => s.StartsWith("ORG:"));
            if (orgString != null)
                vcard.Organization = orgString.Replace("ORG:", "").Trim();

            string langString = contactDetails.FirstOrDefault(s => s.StartsWith("LANG:"));
            if (langString != null)
                vcard.Language = langString.Replace("LANG:", "").Trim();

            string nicknameString = contactDetails.FirstOrDefault(s => s.StartsWith("NICKNAME:"));
            if (nicknameString != null)
                vcard.NickName = nicknameString.Replace("NICKNAME:", "").Trim();

            string birthplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("BIRTHPLACE:"));
            if (birthplaceString != null)
                vcard.BirthPlace = birthplaceString.Replace("BIRTHPLACE:", "").Trim();

            string deathplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("DEATHPLACE:"));
            if (deathplaceString != null)
                vcard.DeathPlace = deathplaceString.Replace("DEATHPLACE:", "").Trim();

            string bdayString = contactDetails.FirstOrDefault(s => s.StartsWith("BDAY:"));
            if (bdayString != null)
            {
                bdayString = bdayString.Replace("BDAY:", "").Trim();
                if(bdayString.Length == 8)
                    vcard.BirthDay = new DateTime(int.Parse(bdayString.Substring(0, 4)), int.Parse(bdayString.Substring(4, 2)), int.Parse(bdayString.Substring(6, 2)));
            }

            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            if (nString != null)
            {
                string[] names = nString.Replace("N:", "").Split(new string[] { ";" }, StringSplitOptions.None);
                if (names.Length > 0)
                    vcard.Firstname = names[0];
                if (names.Length > 1)
                    vcard.Surname = names[1];
                for (int j = 2; j < names.Length; j++)
                {
                    vcard.Othernames = names[j] + " ";
                }
            }
            #endregion

            #region Complex Properties
            string genderString = contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
            if (genderString != null)
            {
                genderString = genderString.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male")
                    vcard.Gender = GenderType.Male;
                else if (genderString.ToLower() == "female")
                    vcard.Gender = GenderType.Female;
                else
                    vcard.Gender = GenderType.Other;
            }
            else
            {
                vcard.Gender = GenderType.None;
            }

            var telStrings = contactDetails.Where(s => s.StartsWith("TEL;"));
            if(telStrings != null)
                foreach(string telString in telStrings)
                {
                    string phoneString = telString.Replace("TEL;", "");
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

            var emailStrings = contactDetails.Where(s => s.StartsWith("EMAIL;"));
            if(emailStrings!=null)
                foreach(string email in emailStrings)
                {
                    string emailString = email.Replace("EMAIL;", "");
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

            //Kind
            string contactKindString = contactDetails.FirstOrDefault(s => s.StartsWith("KIND:"));
            if (contactKindString != null)
            {
                contactKindString = contactKindString.Replace("KIND:", "").Trim();
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

            //Addresses
            var addressStrings = contactDetails.Where(s => s.StartsWith("ADR;"));
            if(addressStrings != null)
                foreach(string addressStr in addressStrings)
                {
                    string addressString = addressStr.Replace("ADR;", "");
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

            //Hobbies
            var hobbyStrings = contactDetails.Where(s => s.StartsWith("HOBBY;"));
            if(hobbyStrings != null)
                foreach(string hobbyStr in hobbyStrings)
                {
                    string hobbyString = hobbyStr.Replace("HOBBY;", "");
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

            //Interests
            var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
            if (interestStrings != null)
                foreach (string interestStr in interestStrings)
                {
                    string interestString = interestStr.Replace("INTEREST;", "");
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

            //Expertises
            var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
            if (expertiseStrings != null)
                foreach (string expertiseStr in expertiseStrings)
                {
                    string expertiseString = expertiseStr.Replace("EXPERTISE;", "");
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

            //Pictures
            var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
            if(photoStrings != null)
                foreach(string photoStr in photoStrings)
                {
                    Photo photo = new Photo();
                    if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:"))
                    {
                        photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Trim();
                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Type = PhotoType.URL;
                        vcard.Pictures.Add(photo);
                    }
                    else if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG;ENCODING=BASE64:"))
                    {
                        string photoString = "";
                        int photoStrIndex = Array.IndexOf(contactDetails, photoStr);
                        while (true)
                        {
                            if (photoStrIndex < contactDetails.Length)
                            {
                                photoString += contactDetails[photoStrIndex];
                                photoStrIndex++;
                                if (contactDetails[photoStrIndex].StartsWith("PHOTO;"))
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        photoString = photoString.Trim();
                        photoString = photoString.Replace("PHOTO;JPEG;ENCODING=BASE64:", "");

                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Picture = GetImageFromBase64String(photoString);
                        photo.Type = PhotoType.Image;
                        vcard.Pictures.Add(photo);
                    }
                }
            #endregion
        }

        private static void ProcessV3_0(ref vCard vcard)
        {
            throw new NotImplementedException("Sorry, support for vcard 3.0 hasn't been implemented");
        }

        private static void ProcessV4_0(ref vCard vcard)
        {
            throw new NotImplementedException("Sorry, support for vcard 4.0 hasn't been implemented");
        }

        private static Bitmap GetImageFromBase64String(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                Bitmap bmp;
                using (var ms = new MemoryStream(imageBytes))
                {
                    bmp = new Bitmap(ms);
                }
                return bmp;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
