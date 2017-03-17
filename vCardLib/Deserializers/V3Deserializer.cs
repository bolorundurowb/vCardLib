using System;
using System.Linq;
using System.Net.Mail;
using vCardLib.Collections;
using vCardLib.Helpers;
using vCardLib.Models;

namespace vCardLib.Deserializers
{
    public class V3Deserializer
    {
        private static string[] _contactDetails;

        /// <summary>
        /// Parse the text representing the vCard object
        /// </summary>
        /// <param name="contactDetailStrings">An array of the vcard properties as strings</param>
        /// <param name="vcard">A partial vcard</param>
        /// <returns>A version 3 vcard object</returns>
        public static vCard Parse(string[] contactDetailStrings, vCard vcard)
        {
            _contactDetails = contactDetailStrings;
            if (vcard == null)
            {
                vcard = new vCard();
            }
            vcard.Addresses = ParseAddresses();
            vcard.EmailAddresses = ParseEmailAddresses();
            vcard.Expertises = ParseExpertises();
            vcard.Hobbies = ParseHobbies();
            vcard.Interests = ParseInterests();
            vcard.PhoneNumbers = ParseTelephoneNumbers();
            vcard.Pictures = ParsePhotos();
            return vcard;
        }

        /// <summary>
        /// Gets the phone numbers from the details array
        /// </summary>
        /// <returns>A <see cref="PhoneNumberCollection"/></returns>
        private static PhoneNumberCollection ParseTelephoneNumbers()
        {
            PhoneNumberCollection phoneNumberCollection = new PhoneNumberCollection();
            
            var telStrings = _contactDetails.Where(s => s.StartsWith("TEL"));
            foreach (string telString in telStrings)
            {
                string phoneString = telString.Replace("TEL;", "").Replace("TEL:", "");
                phoneString = phoneString.Replace("TYPE=", "");
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

                if (phoneString.StartsWith("CELL"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("CELL:", "");
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Cell;
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("HOME"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("HOME:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Home
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("WORK"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("WORK:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Work
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VOICE:"))
                {
                    phoneString = phoneString.Replace("VOICE:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Voice
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("FAX"))
                {
                    phoneString = phoneString.Replace("FAX:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXTPHONE"))
                {
                    phoneString = phoneString.Replace("TEXTPHONE:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXT"))
                {
                    phoneString = phoneString.Replace("TEXT:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Text
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VIDEO"))
                {
                    phoneString = phoneString.Replace("VIDEO:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Video
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("PAGER"))
                {
                    phoneString = phoneString.Replace("PAGER:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MAIN-NUMBER"))
                {
                    phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("BBS"))
                {
                    phoneString = phoneString.Replace("BBS:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("CAR"))
                {
                    phoneString = phoneString.Replace("CAR:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MODEM"))
                {
                    phoneString = phoneString.Replace("MODEM:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("ISDN"))
                {
                    phoneString = phoneString.Replace("ISDN:", "");
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else
                {
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.None
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
            }
            return phoneNumberCollection;
        }

        /// <summary>
        /// Gets the email address from the details array
        /// </summary>
        /// <returns>A <see cref="EmailAddressCollection"/></returns>
        private static EmailAddressCollection ParseEmailAddresses()
        {
            EmailAddressCollection emailAddressCollection = new EmailAddressCollection();
            
            var emailStrings = _contactDetails.Where(s => s.StartsWith("EMAIL"));
            foreach (string email in emailStrings)
            {
                try
                {
                    string emailString = email.Replace("EMAIL;", "").Replace("EMAIL:", "");
                    emailString = emailString.Replace("TYPE=", "");
                    if (emailString.Contains(";"))
                    {
                        emailString = emailString.Replace(";", "");
                    }
                    if (emailString.Contains(","))
                    {
                        int index = emailString.LastIndexOf(",");
                        emailString = emailString.Remove(0, index + 1);
                    }

                    if (emailString.StartsWith("INTERNET:") || emailString.StartsWith("internet:"))
                    {
                        emailString = emailString.Replace("INTERNET:", "").Replace("internet:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Internet
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("HOME:") || emailString.StartsWith("home:"))
                    {
                        emailString = emailString.Replace("HOME:", "").Replace("home:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Home
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("WORK:") || emailString.StartsWith("work:"))
                    {
                        emailString = emailString.Replace("WORK:", "").Replace("work:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Work
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("AOL:") || emailString.StartsWith("aol:"))
                    {
                        emailString = emailString.Replace("AOL:", "").Replace("aol:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.AOL
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("APPLELINK:") || emailString.StartsWith("applelink:"))
                    {
                        emailString = emailString.Replace("APPLELINK:", "").Replace("applelink:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Applelink
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("IBMMAIL:") || emailString.StartsWith("ibmmail:"))
                    {
                        emailString = emailString.Replace("IBMMAIL:", "").Replace("ibmmail:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Work
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else
                    {
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.None
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                }
                catch (FormatException) { }
            }
            return emailAddressCollection;
        }

        /// <summary>
        /// Gets the addresses from the details array
        /// </summary>
        /// <returns>A <see cref="AddressCollection"/></returns>
        private static AddressCollection ParseAddresses()
        {
            AddressCollection addressCollection = new AddressCollection();
            var addressStrings = _contactDetails.Where(s => s.StartsWith("ADR"));
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
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Home
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("WORK:") || addressString.StartsWith("work:"))
                {
                    addressString = addressString.Replace("WORK:", "").Replace("work:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Work
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("DOM:") || addressString.StartsWith("dom:"))
                {
                    addressString = addressString.Replace("DOM:", "").Replace("dom:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Domestic
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("INTL:") || addressString.StartsWith("intl:"))
                {
                    addressString = addressString.Replace("INTL:", "").Replace("intl:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.International
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("PARCEL:") || addressString.StartsWith("parcel:"))
                {
                    addressString = addressString.Replace("PARCEL:", "").Replace("parcel:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Parcel
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("POSTAL:") || addressString.StartsWith("postal:"))
                {
                    addressString = addressString.Replace("POSTAL:", "").Replace("postal:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Postal
                    };
                    addressCollection.Add(address);
                }
                else
                {
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.None
                    };
                    addressCollection.Add(address);
                }
            }

            return addressCollection;
        }

        /// <summary>
        /// Gets the hobbies from the details array
        /// </summary>
        /// <returns>A <see cref="HobbyCollection"/></returns>
        private static HobbyCollection ParseHobbies()
        {
            HobbyCollection hobbyCollection = new HobbyCollection();
            var hobbyStrings = _contactDetails.Where(s => s.StartsWith("HOBBY;"));
            foreach(string hobbyStr in hobbyStrings)
            {
                string hobbyString = hobbyStr.Replace("HOBBY;", "");
                hobbyString = hobbyString.Replace("LEVEL=", "");
                Hobby hobby = new Hobby();
                if (hobbyString.StartsWith("HIGH") || hobbyString.StartsWith("high"))
                {
                    hobby.Level = Level.High;
                    hobby.Activity = hobbyString.Replace("HIGH:", "").Replace("high:", "").Trim();
                    hobbyCollection.Add(hobby);
                }
                else if (hobbyString.StartsWith("MEDIUM") || hobbyString.StartsWith("medium"))
                {
                    hobby.Level = Level.Medium;
                    hobby.Activity = hobbyString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                    hobbyCollection.Add(hobby);
                }
                else if (hobbyString.StartsWith("LOW") || hobbyString.StartsWith("low"))
                {
                    hobby.Level = Level.Low;
                    hobby.Activity = hobbyString.Replace("LOW:", "").Replace("low:", "").Trim();
                    hobbyCollection.Add(hobby);
                }
            }
            return hobbyCollection;
        }

        /// <summary>
        /// Gets the expertises from the details array
        /// </summary>
        /// <returns>A <see cref="ExpertiseCollection"/></returns>
        private static ExpertiseCollection ParseExpertises()
        {
            ExpertiseCollection expertiseCollection = new ExpertiseCollection();
            var expertiseStrings = _contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
            foreach (string expertiseStr in expertiseStrings)
            {
                string expertiseString = expertiseStr.Replace("EXPERTISE;", "");
                expertiseString = expertiseString.Replace("LEVEL=", "");
                Expertise expertise = new Expertise();
                if (expertiseString.StartsWith("HIGH") || expertiseString.StartsWith("high"))
                {
                    expertise.Level = Level.High;
                    expertise.Area = expertiseString.Replace("HIGH:", "").Replace("high:", "").Trim();
                    expertiseCollection.Add(expertise);
                }
                else if (expertiseString.StartsWith("MEDIUM") || expertiseString.StartsWith("medium"))
                {
                    expertise.Level = Level.Medium;
                    expertise.Area = expertiseString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                    expertiseCollection.Add(expertise);
                }
                else if (expertiseString.StartsWith("LOW") || expertiseString.StartsWith("low"))
                {
                    expertise.Level = Level.Low;
                    expertise.Area = expertiseString.Replace("LOW:", "").Replace("low:", "").Trim();
                    expertiseCollection.Add(expertise);
                }
            }
            return expertiseCollection;
        }

        /// <summary>
        /// Gets the interests from the details array
        /// </summary>
        /// <returns>A <see cref="InterestCollection"/></returns>
        private static InterestCollection ParseInterests()
        {
            InterestCollection interestCollection = new InterestCollection();
            var interestStrings = _contactDetails.Where(s => s.StartsWith("INTEREST;"));
            foreach (string interestStr in interestStrings)
            {
                string interestString = interestStr.Replace("INTEREST;", "");
                interestString = interestString.Replace("LEVEL=", "");
                Interest interest = new Interest();
                if (interestString.StartsWith("HIGH") || interestString.StartsWith("high"))
                {
                    interest.Level = Level.High;
                    interest.Activity = interestString.Replace("HIGH:", "").Replace("high:", "").Trim();
                    interestCollection.Add(interest);
                }
                else if (interestString.StartsWith("MEDIUM") || interestString.StartsWith("medium"))
                {
                    interest.Level = Level.Medium;
                    interest.Activity = interestString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
                    interestCollection.Add(interest);
                }
                else if (interestString.StartsWith("LOW") || interestString.StartsWith("low"))
                {
                    interest.Level = Level.Low;
                    interest.Activity = interestString.Replace("LOW:", "").Replace("low:", "").Trim();
                    interestCollection.Add(interest);
                }
            }
            return interestCollection;
        }

        /// <summary>
        /// Gets the photos from the details array
        /// </summary>
        /// <returns>A <see cref="PhotoCollection"/></returns>
        private static PhotoCollection ParsePhotos()
        {
            PhotoCollection photoCollection = new PhotoCollection();
            var photoStrings = _contactDetails.Where(s => s.StartsWith("PHOTO;"));
            foreach (string photoStr in photoStrings)
            {
                var photoString = photoStr.Replace("PHOTO;", "");
                if (photoString.Contains("TYPE=JPEG") || photoString.Contains("TYPE=jpeg"))
                {
                    photoString = photoString
                        .Replace("TYPE=JPEG", "")
                        .Replace("TYPE=jpeg:", "")
                        .Trim();
                    if (photoString.Contains("VALUE=URI") || photoString.Contains("VALUE=uri"))
                    {
                        Photo photo = new Photo
                        {
                            PhotoURL = photoString
                                .Replace("VALUE=URI", "")
                                .Replace("VALUE=uri", "")
                                .Trim(';', ':'),
                            Encoding = PhotoEncoding.JPEG,
                            Type = PhotoType.URL
                        };
                        photoCollection.Add(photo);
                    }
                    else if (photoString.Contains("ENCODING=b"))
                    {
                        int photoStrIndex = Array.IndexOf(_contactDetails, photoStr);
                        while (true)
                        {
                            if (photoStrIndex < _contactDetails.Length)
                            {
                                photoString += _contactDetails[photoStrIndex];
                                photoStrIndex++;
                                if (photoStrIndex < _contactDetails.Length && _contactDetails[photoStrIndex].StartsWith("PHOTO;"))
                                    break;
                            }
                            else
                            {
                                break;
                            }
                        }
                        photoString = photoString
                            .Replace("PHOTO;", "")
                            .Replace("JPEG", "")
                            .Replace("jpeg", "")
                            .Replace("ENCODING=b", "")
                            .Trim(';', ':')
                            .Trim();
                        try
                        {
                            var photo = new Photo
                            {
                                Encoding = PhotoEncoding.JPEG,
                                Picture = Helper.GetImageFromBase64String(photoString),
                                Type = PhotoType.Image
                            };
                            photoCollection.Add(photo);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            //TODO: send error to logger
                        }
                    }
                }
                else if (photoString.Contains("TYPE=GIF") || photoString.Contains("TYPE=gif"))
                {
                    photoString = photoString
                        .Replace("TYPE=URI", "")
                        .Replace("TYPE=uri", "")
                        .Trim();
                    if (photoString.Contains("VALUE=URI") || photoString.Contains("VALUE=uri"))
                    {
                        Photo photo = new Photo
                        {
                            PhotoURL = photoString
                                .Replace("VALUE=URI", "")
                                .Replace("VALUE=uri", "")
                                .Trim(';', ':')
                                .Trim(),
                            Encoding = PhotoEncoding.GIF,
                            Type = PhotoType.URL
                        };
                        photoCollection.Add(photo);
                    }

                }
            }
            return photoCollection;
        }
    }
}
