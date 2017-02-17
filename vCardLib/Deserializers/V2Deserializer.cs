using System;
using System.Linq;
using System.Net.Mail;
using vCardLib.Collections;
using vCardLib.Helpers;
using vCardLib.Models;

namespace vCardLib.Deserializers
{
    public class V2Deserializer
    {
        private static string[] contactDetails;

        public static vCard Parse(string[] contactDetailStrings)
        {
            contactDetails = contactDetailStrings;

            vCard vcard = new vCard
            {
                Addresses = ParseAddresses(),
                BirthDay = ParseBirthDay(),
                BirthPlace = ParseBirthPlace(),
                DeathPlace = ParseDeathPlace(),
                EmailAddresses = ParseEmailAddresses(),
                Expertises = ParseExpertises(),
                FamilyName = ParseFamilyName(),
                FormattedName = ParseFormattedName(),
                Gender = ParseGender(),
                GivenName = ParseGivenName(),
                Hobbies = ParseHobbies(),
                Interests = ParseInterests(),
                Kind = ParseKind(),
                Language = ParseLanguage(),
                MiddleName = ParseMiddleName(),
                NickName = ParseNickname(),
                Organization = ParseOrganization(),
                PhoneNumbers = ParseTelephoneNumbers(),
                Pictures = ParsePhotos(),
                Prefix = ParsePrefix(),
                Suffix = ParseSuffix(),
                TimeZone = ParseTimeZone(),
                Title = ParseTitle(),
                Url = ParseUrl()
            };
            return vcard;
        }

        private static string ParseUrl()
        {
            string urlString = contactDetails.FirstOrDefault(s => s.StartsWith("URL:"));
            if (urlString != null)
                return urlString.Replace("URL:", "").Trim();
            return string.Empty;
        }

        private static string ParseFormattedName()
        {
            string fnString = contactDetails.FirstOrDefault(s => s.StartsWith("FN:"));
            if (fnString != null)
            {
                return fnString.Replace("FN:", "").Trim();
            }
            return string.Empty;
        }

        private static string ParseTitle()
        {
            string titleString = contactDetails.FirstOrDefault(s => s.StartsWith("TITLE:"));
            if (titleString != null)
            {
                return titleString.Replace("TITLE:", "").Trim();
            }
            return string.Empty;
        }

        private static string ParseOrganization()
        {
            string orgString = contactDetails.FirstOrDefault(s => s.StartsWith("ORG:"));
            if (orgString != null)
                return orgString.Replace("ORG:", "").Trim();
            return string.Empty;
        }

        private static string ParseLanguage()
        {
            string langString = contactDetails.FirstOrDefault(s => s.StartsWith("LANG:"));
            if (langString != null)
                return langString.Replace("LANG:", "").Trim();
            return string.Empty;
        }

        private static string ParseNickname()
        {
            string nicknameString = contactDetails.FirstOrDefault(s => s.StartsWith("NICKNAME:"));
            if (nicknameString != null)
                return nicknameString.Replace("NICKNAME:", "").Trim();
            return string.Empty;
        }

        private static string ParseBirthPlace()
        {
            string birthplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("BIRTHPLACE:"));
            if (birthplaceString != null)
                return birthplaceString.Replace("BIRTHPLACE:", "").Trim();
            return string.Empty;
        }

        private static string ParseDeathPlace()
        {
            string deathplaceString = contactDetails.FirstOrDefault(s => s.StartsWith("DEATHPLACE:"));
            if (deathplaceString != null)
                return deathplaceString.Replace("DEATHPLACE:", "").Trim();
            return String.Empty;
        }

        private static DateTime? ParseBirthDay()
        {
            string bdayString = contactDetails.FirstOrDefault(s => s.StartsWith("BDAY:"));
            if (bdayString != null)
            {
                bdayString = bdayString.Replace("BDAY:", "").Replace("-", "").Trim();
                if(bdayString.Length == 8)
                    return  new DateTime(int.Parse(bdayString.Substring(0, 4)), int.Parse(bdayString.Substring(4, 2)), int.Parse(bdayString.Substring(6, 2)));
            }
            return null;
        }

        private static string ParseFamilyName()
        {
            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 0)
                return  names[0];
            return String.Empty;
        }

        private static string ParseGivenName()
        {
            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 1)
                return  names[1];
            return String.Empty;
        }

        private static string ParseMiddleName()
        {
            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 2)
                return  names[2];
            return String.Empty;
        }

        private static string ParsePrefix()
        {
            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 3)
                return  names[3];
            return String.Empty;
        }

        private static string ParseSuffix()
        {
            string nString = contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 4)
                return  names[4];
            return String.Empty;
        }

        private static string ParseTimeZone()
        {
            string tzString = contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                return tzString.Replace("TZ:", "").Trim();
            return String.Empty;
        }

        private static GenderType ParseGender()
        {
            string genderString = contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
            if (genderString != null)
            {
                genderString = genderString.Replace("GENDER:", "").Trim();
                if (genderString.ToLower() == "male" || genderString.ToLower() == "m")
                    return GenderType.Male;
                if (genderString.ToLower() == "female" || genderString.ToLower() == "f")
                    return GenderType.Female;
                return GenderType.Other;
            }
            return GenderType.None;
        }

        private static PhoneNumberCollection ParseTelephoneNumbers()
        {
            PhoneNumberCollection phoneNumberCollection = new PhoneNumberCollection();

            var telStrings = contactDetails.Where(s => s.StartsWith("TEL"));
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
                    PhoneNumber phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Cell
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("HOME"))
                {
                    phoneString = phoneString.Replace(";VOICE", "");
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
                    phoneString = phoneString.Replace(";VOICE", "");
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
                    PhoneNumber phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Text;
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

        private static EmailAddressCollection ParseEmailAddresses()
        {
            EmailAddressCollection emailAddressCollection = new EmailAddressCollection();

            var emailStrings = contactDetails.Where(s => s.StartsWith("EMAIL"));
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
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Internet
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("HOME:"))
                    {
                        emailString = emailString.Replace("HOME:", "");
                        EmailAddress emailAddress = new EmailAddress
                        {
                            Email = new MailAddress(emailString),
                            Type = EmailType.Home
                        };
                        emailAddressCollection.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("WORK:"))
                    {
                        emailString = emailString.Replace("WORK:", "");
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

        private static ContactType ParseKind()
        {
            string contactKindString = contactDetails.FirstOrDefault(s => s.StartsWith("KIND:"));
            if (contactKindString != null)
            {
                contactKindString = contactKindString.Replace("KIND:", "").Trim();
                if (contactKindString == "INDIVIDUAL")
                    return ContactType.Individual;
                if (contactKindString == "GROUP")
                    return ContactType.Group;
                if (contactKindString == "ORG")
                    return ContactType.Organization;
                if (contactKindString == "LOCATION")
                    return ContactType.Location;
                if (contactKindString == "APPLICATION")
                    return ContactType.Application;
                if (contactKindString == "DEVICE")
                    return ContactType.Device;
            }
            return ContactType.Individual;
        }

        private static AddressCollection ParseAddresses()
        {
            AddressCollection addressCollection = new AddressCollection();
            var addressStrings = contactDetails.Where(s => s.StartsWith("ADR"));
            foreach(string addressStr in addressStrings)
            {
                string addressString = addressStr.Replace("ADR;", "").Replace("ADR:", "");
                if (addressString.StartsWith("HOME:"))
                {
                    addressString = addressString.Replace("HOME:", "");
                    Address address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Home
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("WORK:"))
                {
                    addressString = addressString.Replace("WORK:", "");
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

        private static HobbyCollection ParseHobbies()
        {
            HobbyCollection hobbyCollection = new HobbyCollection();
            var hobbyStrings = contactDetails.Where(s => s.StartsWith("HOBBY;"));
            foreach(string hobbyStr in hobbyStrings)
            {
                string hobbyString = hobbyStr.Replace("HOBBY;", "");
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

        private static ExpertiseCollection ParseExpertises()
        {
            ExpertiseCollection expertiseCollection = new ExpertiseCollection();
            var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
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

        private static InterestCollection ParseInterests()
        {
            InterestCollection interestCollection = new InterestCollection();
            var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
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

        private static PhotoCollection ParsePhotos()
        {
            PhotoCollection photoCollection = new PhotoCollection();
            var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
            foreach(string photoStr in photoStrings)
            {
                Photo photo = new Photo();
                if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:"))
                {
                    photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Trim();
                    photo.Encoding = PhotoEncoding.JPEG;
                    photo.Type = PhotoType.URL;
                    photoCollection.Add(photo);
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
                    photo.Picture = Helper.GetImageFromBase64String(photoString);
                    photo.Type = PhotoType.Image;
                    photoCollection.Add(photo);
                }

                else if (photoStr.Replace("PHOTO;", "").StartsWith("GIF:"))
                {
                    photo.PhotoURL = photoStr.Replace("PHOTO;GIF:", "").Trim();
                    photo.Encoding = PhotoEncoding.GIF;
                    photo.Type = PhotoType.URL;
                    photoCollection.Add(photo);
                }
            }
            return photoCollection;
        }
    }
}
