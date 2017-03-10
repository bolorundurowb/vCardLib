using System;
using System.Globalization;
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
        public static vCard Parse(string[] contactDetailStrings)
        {
            _contactDetails = contactDetailStrings;
            
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
                Geo = ParseGeo(),
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
                Revision = ParseRevision(),
                Suffix = ParseSuffix(),
                TimeZone = ParseTimeZone(),
                Title = ParseTitle(),
                Url = ParseUrl(),
                XSkypeDisplayName = ParseXSkypeDisplayName(),
                XSkypePstnNumber = ParseXSkypePstnNumber()
            };
            return vcard;
        }
        

        private static string ParseUrl()
        {
            string urlString = _contactDetails.FirstOrDefault(s => s.StartsWith("URL:"));
            if (urlString != null)
                return urlString.Replace("URL:", "").Trim();
            return string.Empty;
        }

        private static string ParseFormattedName()
        {
            string fnString = _contactDetails.FirstOrDefault(s => s.StartsWith("FN:"));
            if (fnString != null)
            {
                return fnString.Replace("FN:", "").Trim();
            }
            return string.Empty;
        }

        private static string ParseTitle()
        {
            string titleString = _contactDetails.FirstOrDefault(s => s.StartsWith("TITLE:"));
            if (titleString != null)
            {
                return titleString.Replace("TITLE:", "").Trim();
            }
            return string.Empty;
        }

        private static string ParseOrganization()
        {
            string orgString = _contactDetails.FirstOrDefault(s => s.StartsWith("ORG:"));
            if (orgString != null)
                return orgString.Replace("ORG:", "").Trim();
            return string.Empty;
        }

        private static string ParseLanguage()
        {
            string langString = _contactDetails.FirstOrDefault(s => s.StartsWith("LANG:"));
            if (langString != null)
                return langString.Replace("LANG:", "").Trim();
            return string.Empty;
        }

        private static string ParseNickname()
        {
            string nicknameString = _contactDetails.FirstOrDefault(s => s.StartsWith("NICKNAME:"));
            if (nicknameString != null)
                return nicknameString.Replace("NICKNAME:", "").Trim();
            return string.Empty;
        }

        private static string ParseBirthPlace()
        {
            string birthplaceString = _contactDetails.FirstOrDefault(s => s.StartsWith("BIRTHPLACE:"));
            if (birthplaceString != null)
                return birthplaceString.Replace("BIRTHPLACE:", "").Trim();
            return string.Empty;
        }

        private static string ParseDeathPlace()
        {
            string deathplaceString = _contactDetails.FirstOrDefault(s => s.StartsWith("DEATHPLACE:"));
            if (deathplaceString != null)
                return deathplaceString.Replace("DEATHPLACE:", "").Trim();
            return String.Empty;
        }

        private static DateTime? ParseBirthDay()
        {
            string bdayString = _contactDetails.FirstOrDefault(s => s.StartsWith("BDAY:"));
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
            string nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 0)
                return  names[0];
            return String.Empty;
        }

        private static string ParseGivenName()
        {
            string nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 1)
                return  names[1];
            return String.Empty;
        }

        private static string ParseMiddleName()
        {
            string nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 2)
                return  names[2];
            return String.Empty;
        }

        private static string ParsePrefix()
        {
            string nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 3)
                return  names[3];
            return String.Empty;
        }

        private static string ParseSuffix()
        {
            string nString = _contactDetails.FirstOrDefault(s => s.StartsWith("N:"));
            var names = nString?.Replace("N:", "").Split(new[] { ";" }, StringSplitOptions.None);
            if (names?.Length > 4)
                return  names[4];
            return String.Empty;
        }

        private static string ParseTimeZone()
        {
            string tzString = _contactDetails.FirstOrDefault(s => s.StartsWith("TZ"));
            if (tzString != null)
                return tzString.Replace("TZ:", "").Trim();
            return String.Empty;
        }

        private static GenderType ParseGender()
        {
            string genderString = _contactDetails.FirstOrDefault(s => s.StartsWith("GENDER:"));
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

        private static ContactType ParseKind()
        {
            string contactKindString = _contactDetails.FirstOrDefault(s => s.StartsWith("KIND:"));
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

        private static PhotoCollection ParsePhotos()
        {
            PhotoCollection photoCollection = new PhotoCollection();
            var photoStrings = _contactDetails.Where(s => s.StartsWith("PHOTO;"));
            foreach (string photoStr in photoStrings)
            {
                Photo photo = new Photo();
                if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:") || photoStr.Replace("PHOTO;", "").StartsWith("jpeg:"))
                {
                    photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Replace("PHOTO;jpeg:", "").Trim();
                    photo.Encoding = PhotoEncoding.JPEG;
                    photo.Type = PhotoType.URL;
                    photoCollection.Add(photo);
                }
                else if (photoStr.Contains("JPEG") || photoStr.Contains("jpeg") && photoStr.Contains("ENCODING=b"))
                {
                    string photoString = "";
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
                    photoString = photoString.Trim();
                    photoString = photoString.Replace("PHOTO;", "");
                    photoString = photoString.Replace("JPEG", "").Replace("jpeg", "");
                    photoString = photoString.Replace("ENCODING=b", "");
                    photoString = photoString.Trim(';', ':');

                    photo.Encoding = PhotoEncoding.JPEG;
                    photo.Picture = Helper.GetImageFromBase64String(photoString);
                    photo.Type = PhotoType.Image;
                    photoCollection.Add(photo);
                }

                else if (photoStr.Replace("PHOTO;", "").StartsWith("GIF:") || photoStr.Replace("PHOTO;", "").StartsWith("gif:"))
                {
                    photo.PhotoURL = photoStr.Replace("PHOTO;GIF:", "").Replace("PHOTO;gif:", "").Trim();
                    photo.Encoding = PhotoEncoding.GIF;
                    photo.Type = PhotoType.URL;
                    photoCollection.Add(photo);
                }
            }
            return photoCollection;
        }

        public static Geo ParseGeo()
        {
            var geoString = _contactDetails.FirstOrDefault(x => x.StartsWith("GEO"));
            if (geoString != null)
            {
                geoString = geoString.Replace("GEO:", "");
                var geoParts = geoString.Split(';');
                if (geoParts.Length == 2)
                {
                    double longitude;
                    var longSuccess = double.TryParse(geoParts[0], out longitude);
                    double latitude;
                    var latSuccess = double.TryParse(geoParts[1], out latitude);
                    if (longSuccess && latSuccess)
                    {
                        Geo geo = new Geo();
                        geo.Latitude = latitude;
                        geo.Longitude = latitude;
                        return geo;
                    }
                }
            }
            return null;
        }

        private static string ParseXSkypeDisplayName()
        {
            var xSkypeDisplayNumberString = _contactDetails.FirstOrDefault(x => x.StartsWith("X-SKYPE-DISPLAYNAME"));
            if (xSkypeDisplayNumberString != null)
            {
                return xSkypeDisplayNumberString.Replace("X-SKYPE-DISPLAYNAME:", "");
            }
            return String.Empty;
        }

        private static string ParseXSkypePstnNumber()
        {
            var xSkypePstnString = _contactDetails.FirstOrDefault(x => x.StartsWith("X-SKYPE-PSTNNUMBER"));
            if (xSkypePstnString != null)
            {
                return xSkypePstnString.Replace("X-SKYPE-PSTNNUMBER:", "");
            }
            return String.Empty;
        }

        private static DateTime? ParseRevision()
        {
            string revisionString = _contactDetails.FirstOrDefault(x => x.StartsWith("REV"));
            if (revisionString != null)
            {
                revisionString = revisionString.Replace("REV:", "");
                DateTime revision;
                string format = "yyyyMMddTHHmmssZ";
                var dateTimeStyle = DateTimeStyles.None;
                IFormatProvider provider = new CultureInfo("en-US", true);
                if (DateTime.TryParseExact(revisionString, format, provider, dateTimeStyle, out revision))
                {
                    return revision;
                }
            }
            return null;
        }
    }
}
