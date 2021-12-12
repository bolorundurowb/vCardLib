using System;
using System.Collections.Generic;
using System.Linq;
using vCardLib.Constants;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Utils;

namespace vCardLib.Deserializers
{
    // ReSharper disable once InconsistentNaming
    public class v3Deserializer : Deserializer
    {
        protected override vCardVersion ParseVersion()
        {
            return vCardVersion.V3;
        }

        protected override List<Address> ParseAddresses(string[] contactDetails)
        {
            var addressCollection = new List<Address>();
            var addressStrings = contactDetails.Where(s => s.StartsWith("ADR"));
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

        protected override List<TelephoneNumber> ParsePhoneNumbers(IEnumerable<string> contactDetails)
        {
            var phoneNumbers = new List<TelephoneNumber>();

            var telStrings = contactDetails.Where(s => s.StartsWith(FieldKeyConstants.TelKey));
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
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Cell
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("HOME"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("HOME:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Home
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("WORK"))
                {
                    phoneString = phoneString.Replace(",VOICE", "");
                    phoneString = phoneString.Replace("WORK:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Work
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VOICE:"))
                {
                    phoneString = phoneString.Replace("VOICE:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Voice
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("FAX"))
                {
                    phoneString = phoneString.Replace("FAX:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Fax
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXTPHONE"))
                {
                    phoneString = phoneString.Replace("TEXTPHONE:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Fax
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("TEXT"))
                {
                    phoneString = phoneString.Replace("TEXT:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Text
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("VIDEO"))
                {
                    phoneString = phoneString.Replace("VIDEO:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Video
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("PAGER"))
                {
                    phoneString = phoneString.Replace("PAGER:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Pager
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MAIN-NUMBER"))
                {
                    phoneString = phoneString.Replace("MAIN-NUMBER:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Fax
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("BBS"))
                {
                    phoneString = phoneString.Replace("BBS:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Pager
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("CAR"))
                {
                    phoneString = phoneString.Replace("CAR:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Pager
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("MODEM"))
                {
                    phoneString = phoneString.Replace("MODEM:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Pager
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else if (phoneString.StartsWith("ISDN"))
                {
                    phoneString = phoneString.Replace("ISDN:", "");
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.Pager
                    };
                    phoneNumbers.Add(phoneNumber);
                }
                else
                {
                    var phoneNumber = new TelephoneNumber
                    {
                        Value = phoneString,
                        Type = TelephoneNumberType.None
                    };
                    phoneNumbers.Add(phoneNumber);
                }
            }

            return phoneNumbers;
        }

        protected override List<EmailAddress> ParseEmailAddresses(IEnumerable<string> contactDetails)
        {
            var emailAddresses = new List<EmailAddress>();

            var emailStrings = contactDetails.Where(s => s.StartsWith(FieldKeyConstants.EmailKey));
            foreach (var email in emailStrings)
            {
                var emailParts = email.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

                if (emailParts.Length < 1)
                    continue;

                var emailAddress = new EmailAddress { Value = emailParts[1] };

                var metadata = emailParts[0].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                // parse the type info
                var typeMetadata = metadata.Where(x =>
                    x.StartsWith(FieldKeyConstants.TypeKey, StringComparison.OrdinalIgnoreCase));

                foreach (var type in typeMetadata)
                    emailAddress.Type |= EnumHelpers.ParseEmailType(type.Split('=')[1]);

                // parse the email preference
                var preferenceMetadata = metadata.FirstOrDefault(x => x.StartsWith(FieldKeyConstants.PreferenceKey));
                var prefSplit = preferenceMetadata?.Split('=');

                if (prefSplit?.Length > 1)
                {
                    int.TryParse(prefSplit[1], out var preference);
                    if (preference != default)
                        emailAddress.Preference = preference;
                }

                emailAddresses.Add(emailAddress);
            }

            return emailAddresses;
        }

        protected override List<Hobby> ParseHobbies(string[] contactDetails)
        {
            var hobbyCollection = new List<Hobby>();
            var hobbyStrings = contactDetails.Where(s => s.StartsWith("HOBBY;"));
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

        protected override List<Expertise> ParseExpertises(string[] contactDetails)
        {
            var expertiseCollection = new List<Expertise>();
            var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
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

        protected override List<Interest> ParseInterests(string[] contactDetails)
        {
            var interestCollection = new List<Interest>();
            var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
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

        protected override List<Photo> ParsePhotos(string[] contactDetails)
        {
            var photoCollection = new List<Photo>();
            var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
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
                        var photoStrIndex = Array.IndexOf(contactDetails, photoStr);
                        while (true)
                        {
                            if (++photoStrIndex < contactDetails.Length &&
                                contactDetails[photoStrIndex].StartsWith("PHOTO;"))
                            {
                                photoString += contactDetails[photoStrIndex];
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