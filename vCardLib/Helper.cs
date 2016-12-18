using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace vCardLib
{
    /// <summary>
    /// Class to holder all suppoting methods
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Creates a stream reader from supplied file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>A streamreader object</returns>
        /// <exception cref="ArgumentNullException">Supplied path is null or empty</exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static StreamReader GetStreamReaderFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("The filepath supplied is null or empty");
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The specified file at the filepath does not exist");
            return new StreamReader(filePath);
        }

        /// <summary>
        /// Converts the stream reader to a string
        /// </summary>
        /// <param name="streamReader">A valid stream reader object</param>
        /// <returns>A string containing the  text in the stream</returns>
        /// <exception cref="ArgumentNullException">The stream provided was null</exception>
        public static string GetStringFromStreamReader(StreamReader streamReader)
        {
            if (streamReader == null)
                throw new ArgumentNullException("The input stream cannot be null");
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// Splits a single contact string into individual contact strings
        /// </summary>
        /// <param name="contactsString">A string representation of the vcard</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The input is null or empty</exception>
        /// <exception cref="InvalidOperationException">The string does not start and end with appropriate tags</exception>
        public static string[] GetContactsArrayFromString(string contactsString)
        {
            if (string.IsNullOrWhiteSpace(contactsString))
                throw new ArgumentException("string cannot be null, empty or composed of only whitespace characters");
            if (!(contactsString.Contains("BEGIN:VCARD") && contactsString.Contains("END:VCARD")))
                throw new InvalidOperationException("The vcard file does not seem to be a valid vcard file");
            contactsString = contactsString.Replace("BEGIN:VCARD", "");
            return contactsString.Split(new[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetContactDetailsArrayFromString(string contactString)
        {
            contactString = contactString.Replace("PREF;", "").Replace("pref;", "");
            contactString = contactString.Replace("PREF,", "").Replace("pref,", "");
            contactString = contactString.Replace(",PREF", "").Replace(",pref", "");
            return contactString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static vCard GetVcardFromDetails(string[] contactDetails)
        {
            string versionString = contactDetails.FirstOrDefault(s => s.StartsWith("VERSION:"));
            if (versionString == null)
                throw new InvalidOperationException("details do not contain a specification for 'Version'.");
            vCard vcard = new vCard();
            vcard.Version = float.Parse(versionString.Replace("VERSION:", "").Trim());
            if (vcard.Version.Equals(2.1F))
                ProcessV2_1(ref vcard, contactDetails);
            else if (vcard.Version.Equals(3.0F))
                ProcessV3_0(ref vcard, contactDetails);
            else if (vcard.Version.Equals(4.0F))
                ProcessV4_0(ref vcard, contactDetails);
            return vcard;
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
                bdayString = bdayString.Replace("BDAY:", "").Replace("-", "").Trim();
                if(bdayString.Length == 8)
                    vcard.BirthDay = new DateTime(int.Parse(bdayString.Substring(0, 4)), int.Parse(bdayString.Substring(4, 2)), int.Parse(bdayString.Substring(6, 2)));
            }

            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            if (nString != null)
            {
                var names = nString.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
                if (names.Length > 0)
                    vcard.Firstname = names[0];
                if (names.Length > 1)
                    vcard.Surname = names[1];
                for (int j = 2; j < names.Length; j++)
                {
                    vcard.Othernames = names[j] + " ";
                }
            }

            string tzString = contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                vcard.TimeZone = tzString.Replace("TZ:", "").Trim();
            #endregion

            #region Complex Properties
            string genderString = contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
            if (genderString != null)
            {
                genderString = genderString.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male" || genderString.ToLower() == "m")
                    vcard.Gender = GenderType.Male;
                else if (genderString.ToLower() == "female" || genderString.ToLower() == "f")
                    vcard.Gender = GenderType.Female;
                else
                    vcard.Gender = GenderType.Other;
            }
            else
            {
                vcard.Gender = GenderType.None;
            }

            var telStrings = contactDetails.Where(s => s.StartsWith("TEL"));
            if(telStrings != null)
                foreach(string telString in telStrings)
                {
                    string phoneString = telString.Replace("TEL;", "").Replace("TEL:", "");
                    //Remove multiple typing
                    if (phoneString.Contains(";"))
                    {
                        int index = phoneString.LastIndexOf(";");
                        phoneString = phoneString.Remove(0, index + 1);
                    }

                    //Logic
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
					else if (phoneString.StartsWith("TEXTPHONE"))
					{
						phoneString = phoneString.Replace("TEXTPHONE:", "");
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
                    else if (phoneString.StartsWith("MAIN-NUMBER"))
                    {
                        phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Fax;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("BBS"))
                    {
                        phoneString = phoneString.Replace("BBS:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("CAR"))
                    {
                        phoneString = phoneString.Replace("CAR:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("MODEM"))
                    {
                        phoneString = phoneString.Replace("MODEM:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("ISDN"))
                    {
                        phoneString = phoneString.Replace("ISDN:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else
                    {
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.None;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                }

            var emailStrings = contactDetails.Where(s => s.StartsWith("EMAIL"));
            if (emailStrings != null)
                foreach (string email in emailStrings)
                {
                    try
                    {
                        string emailString = email.Replace("EMAIL;", "").Replace("EMAIL:", "");
                        //Remove multiple typing
                        if (emailString.Contains(";"))
                        {
							emailString = emailString.Replace(";", "");
                        }

                        //Logic
                        if (emailString.StartsWith("INTERNET:"))
                        {
                            emailString = emailString.Replace("INTERNET:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Internet;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("HOME:"))
                        {
                            emailString = emailString.Replace("HOME:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Home;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("WORK:"))
                        {
                            emailString = emailString.Replace("WORK:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Work;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("AOL:") || emailString.StartsWith("aol:"))
                        {
                            emailString = emailString.Replace("AOL:", "").Replace("aol:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.AOL;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("APPLELINK:") || emailString.StartsWith("applelink:"))
                        {
                            emailString = emailString.Replace("APPLELINK:", "").Replace("applelink:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Applelink;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("IBMMAIL:") || emailString.StartsWith("ibmmail:"))
                        {
                            emailString = emailString.Replace("IBMMAIL:", "").Replace("ibmmail:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Work;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else
                        {
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.None;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                    }
                    catch (FormatException) { }
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
            var addressStrings = contactDetails.Where(s => s.StartsWith("ADR"));
            if(addressStrings != null)
                foreach(string addressStr in addressStrings)
                {
                    string addressString = addressStr.Replace("ADR;", "").Replace("ADR:", "");
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
                    else if (addressString.StartsWith("DOM:") || addressString.StartsWith("dom:"))
                    {
                        addressString = addressString.Replace("DOM:", "").Replace("dom:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Domestic;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("INTL:") || addressString.StartsWith("intl:"))
                    {
                        addressString = addressString.Replace("INTL:", "").Replace("intl:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.International;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("PARCEL:") || addressString.StartsWith("parcel:"))
                    {
                        addressString = addressString.Replace("PARCEL:", "").Replace("parcel:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Parcel;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("POSTAL:") || addressString.StartsWith("postal:"))
                    {
                        addressString = addressString.Replace("POSTAL:", "").Replace("postal:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Postal;
                        vcard.Addresses.Add(address);
                    }
                    else
                    {
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.None;
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
                    if (hobbyString.StartsWith("HIGH") || hobbyString.StartsWith("high"))
                    {
                        hobby.Level = Level.High;
                        hobby.Activity = hobbyString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                    else if (hobbyString.StartsWith("MEDIUM") || hobbyString.StartsWith("medium"))
                    {
                        hobby.Level = Level.Medium;
                        hobby.Activity = hobbyString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                    else if (hobbyString.StartsWith("LOW") || hobbyString.StartsWith("low"))
                    {
                        hobby.Level = Level.Low;
                        hobby.Activity = hobbyString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                }

            //Interests
            var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
            if (interestStrings != null)
                foreach (string interestStr in interestStrings)
                {
                    string interestString = interestStr.Replace("INTEREST;", "");
                    interestString = interestString.Replace("LEVEL=", "");
                    Interest interest = new Interest();
                    if (interestString.StartsWith("HIGH") || interestString.StartsWith("high"))
                    {
                        interest.Level = Level.High;
                        interest.Activity = interestString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                    else if (interestString.StartsWith("MEDIUM") || interestString.StartsWith("medium"))
                    {
                        interest.Level = Level.Medium;
                        interest.Activity = interestString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                    else if (interestString.StartsWith("LOW") || interestString.StartsWith("low"))
                    {
                        interest.Level = Level.Low;
                        interest.Activity = interestString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                }

            //Expertises
            var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
            if (expertiseStrings != null)
                foreach (string expertiseStr in expertiseStrings)
                {
                    string expertiseString = expertiseStr.Replace("EXPERTISE;", "");
                    expertiseString = expertiseString.Replace("LEVEL=", "");
                    Expertise expertise = new Expertise();
                    if (expertiseString.StartsWith("HIGH") || expertiseString.StartsWith("high"))
                    {
                        expertise.Level = Level.High;
                        expertise.Area = expertiseString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                    else if (expertiseString.StartsWith("MEDIUM") || expertiseString.StartsWith("medium"))
                    {
                        expertise.Level = Level.Medium;
                        expertise.Area = expertiseString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                    else if (expertiseString.StartsWith("LOW") || expertiseString.StartsWith("low"))
                    {
                        expertise.Level = Level.Low;
                        expertise.Area = expertiseString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                }

            //Pictures
            var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
            if(photoStrings != null)
                foreach(string photoStr in photoStrings)
                {
                    Photo photo = new Photo();

                    //JPEG
                    if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:"))
                    {
                        photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Trim();
                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Type = PhotoType.URL;
                        vcard.Pictures.Add(photo);
                    }
                    else if (photoStr.Contains("JPEG") && photoStr.Contains("ENCODING=BASE64"))
                    {
                        string photoString = "";
                        int photoStrIndex = Array.IndexOf(contactDetails, photoStr);
                        while (true)
                        {
                            if (photoStrIndex < contactDetails.Length)
                            {
                                photoString += contactDetails[photoStrIndex];
                                photoStrIndex++;
                                if (photoStrIndex < contactDetails.Length && contactDetails[photoStrIndex].StartsWith("PHOTO;"))
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        photoString = photoString.Trim();
                        photoString = photoString.Replace("PHOTO;", "");
                        photoString = photoString.Replace("JPEG", "");
                        photoString = photoString.Replace("ENCODING=BASE64", "");
                        photoString = photoString.Trim(';', ':');

                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Picture = GetImageFromBase64String(photoString);
                        photo.Type = PhotoType.Image;
                        vcard.Pictures.Add(photo);
                    }

                    //GIF
                    else if (photoStr.Replace("PHOTO;", "").StartsWith("GIF:"))
                    {
                        photo.PhotoURL = photoStr.Replace("PHOTO;GIF:", "").Trim();
                        photo.Encoding = PhotoEncoding.GIF;
                        photo.Type = PhotoType.URL;
                        vcard.Pictures.Add(photo);
                    }
                }
            #endregion
        }

        private static void ProcessV3_0(ref vCard vcard, string[] contactDetails)
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
                vcard.Organization = orgString.Replace("ORG:", "").Replace(";", " ").Trim();

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
                bdayString = bdayString.Replace("BDAY:", "").Replace("-", "").Trim();
                if (bdayString.Length == 8)
                    vcard.BirthDay = new DateTime(int.Parse(bdayString.Substring(0, 4)), int.Parse(bdayString.Substring(4, 2)), int.Parse(bdayString.Substring(6, 2)));
            }

            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            if (nString != null)
            {
                string[] names = nString.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
                if (names.Length > 0)
                    vcard.Firstname = names[0];
                if (names.Length > 1)
                    vcard.Surname = names[1];
                for (int j = 2; j < names.Length; j++)
                {
                    vcard.Othernames = names[j] + " ";
                }
            }

            string tzString = contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                vcard.TimeZone = tzString.Replace("TZ:", "").Trim();
            #endregion
                
            #region Complex Properties
            string genderString = contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
            if (genderString != null)
            {
                genderString = genderString.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male" || genderString.ToLower() == "m")
                    vcard.Gender = GenderType.Male;
				else if (genderString.ToLower() == "female" || genderString.ToLower() == "f")
                    vcard.Gender = GenderType.Female;
                else
                    vcard.Gender = GenderType.Other;
            }
            else
            {
                vcard.Gender = GenderType.None;
            }

            var telStrings = contactDetails.Where(s => s.StartsWith("TEL"));
            if (telStrings != null)
                foreach (string telString in telStrings)
                {
                    string phoneString = telString.Replace("TEL;", "").Replace("TEL:", "");
                    phoneString = phoneString.Replace("TYPE=", "");
                    //Remove multiple typing
                    if (phoneString.Contains(";"))
                    {
                        int index = phoneString.LastIndexOf(";");
                        phoneString = phoneString.Remove(0, index + 1);
                    }
                    if (phoneString.Contains(","))
                    {
                        int index = phoneString.LastIndexOf(",");
                        phoneString = phoneString.Remove(0, index + 1);
                    }

                    //Logic
                    if (phoneString.StartsWith("CELL"))
                    {
                        phoneString = phoneString.Replace(",VOICE", "");
                        phoneString = phoneString.Replace("CELL:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Cell;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("HOME"))
                    {
                        phoneString = phoneString.Replace(",VOICE", "");
                        phoneString = phoneString.Replace("HOME:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Home;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("WORK"))
                    {
                        phoneString = phoneString.Replace(",VOICE", "");
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
					else if (phoneString.StartsWith("TEXTPHONE"))
					{
						phoneString = phoneString.Replace("TEXTPHONE:", "");
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
                    else if (phoneString.StartsWith("MAIN-NUMBER"))
                    {
                        phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Fax;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("BBS"))
                    {
                        phoneString = phoneString.Replace("BBS:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("CAR"))
                    {
                        phoneString = phoneString.Replace("CAR:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("MODEM"))
                    {
                        phoneString = phoneString.Replace("MODEM:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else if (phoneString.StartsWith("ISDN"))
                    {
                        phoneString = phoneString.Replace("ISDN:", "");
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.Pager;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                    else
                    {
                        PhoneNumber phoneNumber = new PhoneNumber();
                        phoneNumber.Number = phoneString;
                        phoneNumber.Type = PhoneNumberType.None;
                        vcard.PhoneNumbers.Add(phoneNumber);
                    }
                }

            var emailStrings = contactDetails.Where(s => s.StartsWith("EMAIL"));
            if (emailStrings != null)
                foreach (string email in emailStrings)
                {
                    try
                    {
                        string emailString = email.Replace("EMAIL;", "").Replace("EMAIL:", "");
                        emailString = emailString.Replace("TYPE=", "");
                        //Remove multiple typing
                        if (emailString.Contains(";"))
                        {
							emailString = emailString.Replace(";", "");
                        }
                        if (emailString.Contains(","))
                        {
                            int index = emailString.LastIndexOf(",");
                            emailString = emailString.Remove(0, index + 1);
                        }

                        //Logic
                        if (emailString.StartsWith("INTERNET:") || emailString.StartsWith("internet:"))
                        {
                            emailString = emailString.Replace("INTERNET:", "").Replace("internet:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Internet;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("HOME:") || emailString.StartsWith("home:"))
                        {
                            emailString = emailString.Replace("HOME:", "").Replace("home:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Home;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("WORK:") || emailString.StartsWith("work:"))
                        {
                            emailString = emailString.Replace("WORK:", "").Replace("work:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Work;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("AOL:") || emailString.StartsWith("aol:"))
                        {
                            emailString = emailString.Replace("AOL:", "").Replace("aol:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.AOL;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("APPLELINK:") || emailString.StartsWith("applelink:"))
                        {
                            emailString = emailString.Replace("APPLELINK:", "").Replace("applelink:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Applelink;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else if (emailString.StartsWith("IBMMAIL:") || emailString.StartsWith("ibmmail:"))
                        {
                            emailString = emailString.Replace("IBMMAIL:", "").Replace("ibmmail:", "");
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.Work;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                        else
                        {
                            EmailAddress emailAddress = new EmailAddress();
                            emailAddress.Email = new MailAddress(emailString);
                            emailAddress.Type = EmailType.None;
                            vcard.EmailAddresses.Add(emailAddress);
                        }
                    }
                    catch (FormatException) { }
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
            var addressStrings = contactDetails.Where(s => s.StartsWith("ADR"));
            if (addressStrings != null)
                foreach (string addressStr in addressStrings)
                {
                    string addressString = addressStr.Replace("ADR;", "").Replace("ADR:", "");
                    addressString = addressString.Replace("TYPE=", "");
                    //Remove multiple typing
                    if (addressString.Contains(","))
                    {
                        int index = addressString.LastIndexOf(",");
                        addressString = addressString.Remove(0, index + 1);
                    }

                    //Logic
                    if (addressString.StartsWith("HOME:") || addressString.StartsWith("home:"))
                    {
                        addressString = addressString.Replace("HOME:", "").Replace("home:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Home;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("WORK:") || addressString.StartsWith("work:"))
                    {
                        addressString = addressString.Replace("WORK:", "").Replace("work:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Work;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("DOM:") || addressString.StartsWith("dom:"))
                    {
                        addressString = addressString.Replace("DOM:", "").Replace("dom:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Domestic;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("INTL:") || addressString.StartsWith("intl:"))
                    {
                        addressString = addressString.Replace("INTL:", "").Replace("intl:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.International;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("PARCEL:") || addressString.StartsWith("parcel:"))
                    {
                        addressString = addressString.Replace("PARCEL:", "").Replace("parcel:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Parcel;
                        vcard.Addresses.Add(address);
                    }
                    else if (addressString.StartsWith("POSTAL:") || addressString.StartsWith("postal:"))
                    {
                        addressString = addressString.Replace("POSTAL:", "").Replace("postal:", "");
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.Postal;
                        vcard.Addresses.Add(address);
                    }
                    else
                    {
                        Address address = new Address();
                        address.Location = addressString.Replace(";", " ");
                        address.Type = AddressType.None;
                        vcard.Addresses.Add(address);
                    }
                }

            //Hobbies
            var hobbyStrings = contactDetails.Where(s => s.StartsWith("HOBBY;"));
            if (hobbyStrings != null)
                foreach (string hobbyStr in hobbyStrings)
                {
                    string hobbyString = hobbyStr.Replace("HOBBY;", "");
                    hobbyString = hobbyString.Replace("LEVEL=", "");
                    Hobby hobby = new Hobby();
                    if (hobbyString.StartsWith("HIGH") || hobbyString.StartsWith("high"))
                    {
                        hobby.Level = Level.High;
                        hobby.Activity = hobbyString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                    else if (hobbyString.StartsWith("MEDIUM") || hobbyString.StartsWith("medium"))
                    {
                        hobby.Level = Level.Medium;
                        hobby.Activity = hobbyString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                    else if (hobbyString.StartsWith("LOW") || hobbyString.StartsWith("low"))
                    {
                        hobby.Level = Level.Low;
                        hobby.Activity = hobbyString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Hobbies.Add(hobby);
                    }
                }

            //Interests
            var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
            if (interestStrings != null)
                foreach (string interestStr in interestStrings)
                {
                    string interestString = interestStr.Replace("INTEREST;", "");
                    interestString = interestString.Replace("LEVEL=", "");
                    Interest interest = new Interest();
                    if (interestString.StartsWith("HIGH") || interestString.StartsWith("high"))
                    {
                        interest.Level = Level.High;
                        interest.Activity = interestString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                    else if (interestString.StartsWith("MEDIUM") || interestString.StartsWith("medium"))
                    {
                        interest.Level = Level.Medium;
                        interest.Activity = interestString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                    else if (interestString.StartsWith("LOW") || interestString.StartsWith("low"))
                    {
                        interest.Level = Level.Low;
                        interest.Activity = interestString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Interests.Add(interest);
                    }
                }

            //Expertises
            var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
            if (expertiseStrings != null)
                foreach (string expertiseStr in expertiseStrings)
                {
                    string expertiseString = expertiseStr.Replace("EXPERTISE;", "");
                    expertiseString = expertiseString.Replace("LEVEL=", "");
                    Expertise expertise = new Expertise();
                    if (expertiseString.StartsWith("HIGH") || expertiseString.StartsWith("high"))
                    {
                        expertise.Level = Level.High;
                        expertise.Area = expertiseString.Replace("HIGH:", "").Replace("high:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                    else if (expertiseString.StartsWith("MEDIUM") || expertiseString.StartsWith("medium"))
                    {
                        expertise.Level = Level.Medium;
                        expertise.Area = expertiseString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                    else if (expertiseString.StartsWith("LOW") || expertiseString.StartsWith("low"))
                    {
                        expertise.Level = Level.Low;
                        expertise.Area = expertiseString.Replace("LOW:", "").Replace("low:", "").Trim();
                        vcard.Expertises.Add(expertise);
                    }
                }

            //Pictures
            var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
            if (photoStrings != null)
                foreach (string photoStr in photoStrings)
                {
                    Photo photo = new Photo();

                    //JPEG
                    if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:") || photoStr.Replace("PHOTO;", "").StartsWith("jpeg:"))
                    {
                        photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Replace("PHOTO;jpeg:", "").Trim();
                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Type = PhotoType.URL;
                        vcard.Pictures.Add(photo);
                    }
                    else if (photoStr.Contains("JPEG") || photoStr.Contains("jpeg") && photoStr.Contains("ENCODING=b"))
                    {
                        string photoString = "";
                        int photoStrIndex = Array.IndexOf(contactDetails, photoStr);
                        while (true)
                        {
                            if (photoStrIndex < contactDetails.Length)
                            {
                                photoString += contactDetails[photoStrIndex];
                                photoStrIndex++;
                                if (photoStrIndex < contactDetails.Length && contactDetails[photoStrIndex].StartsWith("PHOTO;"))
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        photoString = photoString.Trim();
                        photoString = photoString.Replace("PHOTO;", "");
                        photoString = photoString.Replace("JPEG", "").Replace("jpeg", "");
                        photoString = photoString.Replace("ENCODING=b", "");
                        photoString = photoString.Trim(';', ':');

                        photo.Encoding = PhotoEncoding.JPEG;
                        photo.Picture = GetImageFromBase64String(photoString);
                        photo.Type = PhotoType.Image;
                        vcard.Pictures.Add(photo);
                    }

                    //GIF
                    else if (photoStr.Replace("PHOTO;", "").StartsWith("GIF:") || photoStr.Replace("PHOTO;", "").StartsWith("gif:"))
                    {
                        photo.PhotoURL = photoStr.Replace("PHOTO;GIF:", "").Replace("PHOTO;gif:", "").Trim();
                        photo.Encoding = PhotoEncoding.GIF;
                        photo.Type = PhotoType.URL;
                        vcard.Pictures.Add(photo);
                    }
                }
            #endregion
        }

        private static void ProcessV4_0(ref vCard vcard, string[] contactDetails)
        {
            throw new NotImplementedException("Sorry, support for vcard 4.0 hasn't been implemented");
        }

        public static Bitmap GetImageFromBase64String(string base64String)
        {
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                Bitmap bmp;
                using (var ms = new MemoryStream(imageBytes))
                {
					bmp = (Bitmap)Image.FromStream(ms);
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
