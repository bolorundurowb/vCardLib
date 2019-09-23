using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Deserializers
{
    public static class V3Deserializer
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
        /// <returns>A <see cref="List`PhoneNumber"/></returns>
        private static List<PhoneNumber> ParseTelephoneNumbers()
        {
            var phoneNumberCollection = new List<PhoneNumber>();

            var telStrings = _contactDetails.Where(s => s.StartsWith("TEL"));
            foreach (var telString in telStrings)
            {
                var phoneString = telString.Replace("TEL;", "").Replace("TEL:", "");
                phoneString = phoneString.Replace("TYPE=", "");
                if (phoneString.Contains(";"))
                {
                    var index = phoneString.LastIndexOf(";", StringComparison.Ordinal);
                    phoneString = phoneString.Remove(0, index + 1);
                }

                if (phoneString.Contains(","))
                {
                    var index = phoneString.LastIndexOf(",", StringComparison.Ordinal);
                    phoneString = phoneString.Remove(0, index + 1);
                }

                if (phoneString.StartsWith("CELL"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("CELL:", "");
                    var phoneNumber = new PhoneNumber();
                    phoneNumber.Number = phoneString;
                    phoneNumber.Type = PhoneNumberType.Cell;
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("HOME"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("HOME:", "");
                    var phoneNumber = new PhoneNumber
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
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Work
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VOICE:"))
                {
                    phoneString = phoneString.Replace("VOICE:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Voice
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("FAX"))
                {
                    phoneString = phoneString.Replace("FAX:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXTPHONE"))
                {
                    phoneString = phoneString.Replace("TEXTPHONE:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXT"))
                {
                    phoneString = phoneString.Replace("TEXT:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Text
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VIDEO"))
                {
                    phoneString = phoneString.Replace("VIDEO:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Video
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("PAGER"))
                {
                    phoneString = phoneString.Replace("PAGER:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MAIN-NUMBER"))
                {
                    phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Fax
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("BBS"))
                {
                    phoneString = phoneString.Replace("BBS:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("CAR"))
                {
                    phoneString = phoneString.Replace("CAR:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MODEM"))
                {
                    phoneString = phoneString.Replace("MODEM:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("ISDN"))
                {
                    phoneString = phoneString.Replace("ISDN:", "");
                    var phoneNumber = new PhoneNumber
                    {
                        Number = phoneString,
                        Type = PhoneNumberType.Pager
                    };
                    phoneNumberCollection.Add(phoneNumber);
                }
                else
                {
                    var phoneNumber = new PhoneNumber
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
        /// <returns>A <see cref="List`EmailAddress"/></returns>
        private static List<EmailAddress> ParseEmailAddresses()
        {
            var emailAddresses = new List<EmailAddress>();

            var emailStrings = _contactDetails.Where(s => s.StartsWith("EMAIL"));
            foreach (var email in emailStrings)
            {
                try
                {
                    var emailString = email.Replace("EMAIL;", "").Replace("EMAIL:", "");
                    emailString = emailString.Replace("TYPE=", "");
                    if (emailString.Contains(";"))
                    {
                        emailString = emailString.Replace(";", "");
                    }

                    if (emailString.Contains(","))
                    {
                        var index = emailString.LastIndexOf(",");
                        emailString = emailString.Remove(0, index + 1);
                    }

                    if (emailString.StartsWith("INTERNET:") || emailString.StartsWith("internet:"))
                    {
                        emailString = emailString.Replace("INTERNET:", "").Replace("internet:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.Internet
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("HOME:") || emailString.StartsWith("home:"))
                    {
                        emailString = emailString.Replace("HOME:", "").Replace("home:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.Home
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("WORK:") || emailString.StartsWith("work:"))
                    {
                        emailString = emailString.Replace("WORK:", "").Replace("work:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.Work
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("AOL:") || emailString.StartsWith("aol:"))
                    {
                        emailString = emailString.Replace("AOL:", "").Replace("aol:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.AOL
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("APPLELINK:") || emailString.StartsWith("applelink:"))
                    {
                        emailString = emailString.Replace("APPLELINK:", "").Replace("applelink:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.Applelink
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else if (emailString.StartsWith("IBMMAIL:") || emailString.StartsWith("ibmmail:"))
                    {
                        emailString = emailString.Replace("IBMMAIL:", "").Replace("ibmmail:", "");
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.Work
                        };
                        emailAddresses.Add(emailAddress);
                    }
                    else
                    {
                        var emailAddress = new EmailAddress
                        {
                            Email = emailString,
                            Type = EmailType.None
                        };
                        emailAddresses.Add(emailAddress);
                    }
                }
                catch (FormatException)
                {
                }
            }

            return emailAddresses;
        }

        /// <summary>
        /// Gets the addresses from the details array
        /// </summary>
        /// <returns>A <see cref="List`Address"/></returns>
        private static List<Address> ParseAddresses()
        {
            var addressCollection = new List<Address>();
            var addressStrings = _contactDetails.Where(s => s.StartsWith("ADR"));
            foreach (var addressStr in addressStrings)
            {
                var addressString = addressStr.Replace("ADR;", "").Replace("ADR:", "");
                addressString = addressString.Replace("TYPE=", "");
                //Remove multiple typing
                if (addressString.Contains(","))
                {
                    var index = addressString.LastIndexOf(",", StringComparison.Ordinal);
                    addressString = addressString.Remove(0, index + 1);
                }

                //Logic
                if (addressString.StartsWith("HOME:") || addressString.StartsWith("home:"))
                {
                    addressString = addressString.Replace("HOME:", "").Replace("home:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Home
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("WORK:") || addressString.StartsWith("work:"))
                {
                    addressString = addressString.Replace("WORK:", "").Replace("work:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Work
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("DOM:") || addressString.StartsWith("dom:"))
                {
                    addressString = addressString.Replace("DOM:", "").Replace("dom:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Domestic
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("INTL:") || addressString.StartsWith("intl:"))
                {
                    addressString = addressString.Replace("INTL:", "").Replace("intl:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.International
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("PARCEL:") || addressString.StartsWith("parcel:"))
                {
                    addressString = addressString.Replace("PARCEL:", "").Replace("parcel:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Parcel
                    };
                    addressCollection.Add(address);
                }
                else if (addressString.StartsWith("POSTAL:") || addressString.StartsWith("postal:"))
                {
                    addressString = addressString.Replace("POSTAL:", "").Replace("postal:", "");
                    var address = new Address
                    {
                        Location = addressString.Replace(";", " "),
                        Type = AddressType.Postal
                    };
                    addressCollection.Add(address);
                }
                else
                {
                    var address = new Address
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
        /// <returns>A <see cref="List`Hobby"/></returns>
        private static List<Hobby> ParseHobbies()
        {
            var hobbyCollection = new List<Hobby>();
            var hobbyStrings = _contactDetails.Where(s => s.StartsWith("HOBBY;"));
            foreach (var hobbyStr in hobbyStrings)
            {
                var hobbyString = hobbyStr.Replace("HOBBY;", "");
                hobbyString = hobbyString.Replace("LEVEL=", "");
                var hobby = new Hobby();
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
        /// <returns>A <see cref="List`Expertise"/></returns>
        private static List<Expertise> ParseExpertises()
        {
            var expertiseCollection = new List<Expertise>();
            var expertiseStrings = _contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
            foreach (var expertiseStr in expertiseStrings)
            {
                var expertiseString = expertiseStr.Replace("EXPERTISE;", "");
                expertiseString = expertiseString.Replace("LEVEL=", "");
                var expertise = new Expertise();
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
        /// <returns>A <see cref="List`Interest"/></returns>
        private static List<Interest> ParseInterests()
        {
            var interestCollection = new List<Interest>();
            var interestStrings = _contactDetails.Where(s => s.StartsWith("INTEREST;"));
            foreach (var interestStr in interestStrings)
            {
                var interestString = interestStr.Replace("INTEREST;", "");
                interestString = interestString.Replace("LEVEL=", "");
                var interest = new Interest();
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
        /// <returns>A <see cref="List`Photo"/></returns>
        private static List<Photo> ParsePhotos()
        {
            var photoCollection = new List<Photo>();
            var photoStrings = _contactDetails.Where(s => s.StartsWith("PHOTO;"));
            foreach (var photoStr in photoStrings)
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
                        var photo = new Photo
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
                        var photoStrIndex = Array.IndexOf(_contactDetails, photoStr);
                        while (true)
                        {
                            if (photoStrIndex < _contactDetails.Length)
                            {
                                photoString += _contactDetails[photoStrIndex];
                                photoStrIndex++;
                                if (photoStrIndex < _contactDetails.Length &&
                                    _contactDetails[photoStrIndex].StartsWith("PHOTO;"))
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
                                Picture = Convert.FromBase64String(photoString),
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
                        var photo = new Photo
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
