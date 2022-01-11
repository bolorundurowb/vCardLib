// using System;
// using System.Collections.Generic;
// using System.Linq;
// using vCardLib.Constants;
// using vCardLib.Enums;
// using vCardLib.Models;
// using vCardLib.Utils;
//
// namespace vCardLib.Deserializers;
//
// // ReSharper disable once InconsistentNaming
// public sealed class v2Deserializer : Deserializer
// {
//     protected override vCardVersion ParseVersion()
//     {
//         return vCardVersion.v2;
//     }
//
//     protected override List<Address> ParseAddresses(string[] contactDetails)
//     {
//         var addressCollection = new List<Address>();
//         var addressStrings = contactDetails.Where(s => s.StartsWith("ADR"));
//         foreach (var addressStr in addressStrings)
//         {
//             var addressString = addressStr.Replace("ADR;", "").Replace("ADR:", "");
//             if (addressString.StartsWith("HOME:"))
//             {
//                 addressString = addressString.Replace("HOME:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.Home
//                 };
//                 addressCollection.Add(address);
//             }
//             else if (addressString.StartsWith("WORK:"))
//             {
//                 addressString = addressString.Replace("WORK:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.Work
//                 };
//                 addressCollection.Add(address);
//             }
//             else if (addressString.StartsWith("DOM:") || addressString.StartsWith("dom:"))
//             {
//                 addressString = addressString.Replace("DOM:", "").Replace("dom:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.Domestic
//                 };
//                 addressCollection.Add(address);
//             }
//             else if (addressString.StartsWith("INTL:") || addressString.StartsWith("intl:"))
//             {
//                 addressString = addressString.Replace("INTL:", "").Replace("intl:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.International
//                 };
//                 addressCollection.Add(address);
//             }
//             else if (addressString.StartsWith("PARCEL:") || addressString.StartsWith("parcel:"))
//             {
//                 addressString = addressString.Replace("PARCEL:", "").Replace("parcel:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.Parcel
//                 };
//                 addressCollection.Add(address);
//             }
//             else if (addressString.StartsWith("POSTAL:") || addressString.StartsWith("postal:"))
//             {
//                 addressString = addressString.Replace("POSTAL:", "").Replace("postal:", "");
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.Postal
//                 };
//                 addressCollection.Add(address);
//             }
//             else
//             {
//                 var address = new Address
//                 {
//                     Location = addressString.Replace(";", " "),
//                     Type = AddressType.None
//                 };
//                 addressCollection.Add(address);
//             }
//         }
//
//         return addressCollection;
//     }
//
//     protected override List<TelephoneNumber> ParsePhoneNumbers(IEnumerable<string> contactDetails)
//     {
//         var phoneNumbers = new List<TelephoneNumber>();
//
//         var telStrings = contactDetails.Where(s => s.StartsWith(FieldKeyConstants.TelKey));
//         foreach (var tel in telStrings)
//         {
//             var telParts = tel.Split(FieldKeyConstants.SectionDelimiter, StringSplitOptions.RemoveEmptyEntries);
//
//             if (telParts.Length < 1)
//                 continue;
//
//             // parse the phone number and extension
//             var values = telParts.Last().Split(FieldKeyConstants.MetadataDelimiter);
//             var extensionValue = values.FirstOrDefault(x =>
//                 x.StartsWith(TelephoneNumberTypeConstants.Extension, StringComparison.OrdinalIgnoreCase));
//
//             var phoneNumber = new TelephoneNumber
//             {
//                 Value = values.FirstOrDefault(),
//                 Extension = extensionValue?.Split(FieldKeyConstants.KeyValueDelimiter).LastOrDefault()
//             };
//
//             // parse metadata
//             var metadata = telParts.First().Split(FieldKeyConstants.MetadataDelimiter,
//                 StringSplitOptions.RemoveEmptyEntries);
//
//             var typeMetadata = metadata.Where(x =>
//                 x.StartsWith(FieldKeyConstants.TypeKey, StringComparison.OrdinalIgnoreCase));
//
//             foreach (var type in typeMetadata)
//                 phoneNumber.Type |= EnumHelpers.ParseTelephoneType(type);
//
//             phoneNumbers.Add(phoneNumber);
//         }
//
//         return phoneNumbers;
//     }
//
//     protected override List<EmailAddress> ParseEmailAddresses(IEnumerable<string> contactDetails)
//     {
//         var emailAddresses = new List<EmailAddress>();
//
//         var emailStrings = contactDetails.Where(s => s.StartsWith(FieldKeyConstants.EmailKey));
//         foreach (var email in emailStrings)
//         {
//             var emailParts = email.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
//
//             if (emailParts.Length < 1)
//                 continue;
//
//             var emailAddress = new EmailAddress { Value = emailParts[1] };
//
//             var metadata = emailParts[0].Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
//
//             // parse the type info
//             var typeMetadata = metadata.Where(x => !x.Contains(FieldKeyConstants.EmailKey));
//
//             foreach (var type in typeMetadata)
//                 emailAddress.Type |= EnumHelpers.ParseEmailType(type);
//
//             emailAddresses.Add(emailAddress);
//         }
//
//         return emailAddresses;
//     }
//
//     protected override List<Hobby> ParseHobbies(string[] contactDetails)
//     {
//         var hobbyCollection = new List<Hobby>();
//         var hobbyStrings = contactDetails.Where(s => s.StartsWith("HOBBY;"));
//         foreach (var hobbyStr in hobbyStrings)
//         {
//             var hobbyString = hobbyStr.Replace("HOBBY;", "");
//             var hobby = new Hobby();
//             if (hobbyString.StartsWith("HIGH") || hobbyString.StartsWith("high"))
//             {
//                 hobby.Level = Level.High;
//                 hobby.Activity = hobbyString.Replace("HIGH:", "").Replace("high:", "").Trim();
//                 hobbyCollection.Add(hobby);
//             }
//             else if (hobbyString.StartsWith("MEDIUM") || hobbyString.StartsWith("medium"))
//             {
//                 hobby.Level = Level.Medium;
//                 hobby.Activity = hobbyString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
//                 hobbyCollection.Add(hobby);
//             }
//             else if (hobbyString.StartsWith("LOW") || hobbyString.StartsWith("low"))
//             {
//                 hobby.Level = Level.Low;
//                 hobby.Activity = hobbyString.Replace("LOW:", "").Replace("low:", "").Trim();
//                 hobbyCollection.Add(hobby);
//             }
//         }
//
//         return hobbyCollection;
//     }
//
//     protected override List<Expertise> ParseExpertises(string[] contactDetails)
//     {
//         var expertiseCollection = new List<Expertise>();
//         var expertiseStrings = contactDetails.Where(s => s.StartsWith("EXPERTISE;"));
//         foreach (var expertiseStr in expertiseStrings)
//         {
//             var expertiseString = expertiseStr.Replace("EXPERTISE;", "");
//             expertiseString = expertiseString.Replace("LEVEL=", "");
//             var expertise = new Expertise();
//             if (expertiseString.StartsWith("HIGH") || expertiseString.StartsWith("high"))
//             {
//                 expertise.Level = Level.High;
//                 expertise.Area = expertiseString.Replace("HIGH:", "").Replace("high:", "").Trim();
//                 expertiseCollection.Add(expertise);
//             }
//             else if (expertiseString.StartsWith("MEDIUM") || expertiseString.StartsWith("medium"))
//             {
//                 expertise.Level = Level.Medium;
//                 expertise.Area = expertiseString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
//                 expertiseCollection.Add(expertise);
//             }
//             else if (expertiseString.StartsWith("LOW") || expertiseString.StartsWith("low"))
//             {
//                 expertise.Level = Level.Low;
//                 expertise.Area = expertiseString.Replace("LOW:", "").Replace("low:", "").Trim();
//                 expertiseCollection.Add(expertise);
//             }
//         }
//
//         return expertiseCollection;
//     }
//
//     protected override List<Interest> ParseInterests(string[] contactDetails)
//     {
//         var interestCollection = new List<Interest>();
//         var interestStrings = contactDetails.Where(s => s.StartsWith("INTEREST;"));
//         foreach (var interestStr in interestStrings)
//         {
//             var interestString = interestStr.Replace("INTEREST;", "");
//             interestString = interestString.Replace("LEVEL=", "");
//             var interest = new Interest();
//             if (interestString.StartsWith("HIGH") || interestString.StartsWith("high"))
//             {
//                 interest.Level = Level.High;
//                 interest.Activity = interestString.Replace("HIGH:", "").Replace("high:", "").Trim();
//                 interestCollection.Add(interest);
//             }
//             else if (interestString.StartsWith("MEDIUM") || interestString.StartsWith("medium"))
//             {
//                 interest.Level = Level.Medium;
//                 interest.Activity = interestString.Replace("MEDIUM:", "").Replace("medium:", "").Trim();
//                 interestCollection.Add(interest);
//             }
//             else if (interestString.StartsWith("LOW") || interestString.StartsWith("low"))
//             {
//                 interest.Level = Level.Low;
//                 interest.Activity = interestString.Replace("LOW:", "").Replace("low:", "").Trim();
//                 interestCollection.Add(interest);
//             }
//         }
//
//         return interestCollection;
//     }
//
//     protected override List<Photo> ParsePhotos(string[] contactDetails)
//     {
//         var photoCollection = new List<Photo>();
//         var photoStrings = contactDetails.Where(s => s.StartsWith("PHOTO;"));
//         foreach (var photoStr in photoStrings)
//         {
//             var photo = new Photo();
//             if (photoStr.Replace("PHOTO;", "").StartsWith("JPEG:"))
//             {
//                 photo.PhotoURL = photoStr.Replace("PHOTO;JPEG:", "").Trim();
//                 photo.Encoding = PhotoEncoding.JPEG;
//                 photo.Type = PhotoType.URL;
//                 photoCollection.Add(photo);
//             }
//             else if (photoStr.Contains("JPEG") && photoStr.Contains("ENCODING=BASE64"))
//             {
//                 var photoString = photoStr.Trim();
//                 photoString = photoString.Replace("PHOTO;", "");
//                 photoString = photoString.Replace("JPEG", "");
//                 photoString = photoString.Replace("ENCODING=BASE64", "");
//                 photoString = photoString.Trim(';', ':');
//
//                 photo.Encoding = PhotoEncoding.JPEG;
//                 photo.Picture = Convert.FromBase64String(photoString);
//                 photo.Type = PhotoType.Image;
//                 photoCollection.Add(photo);
//             }
//
//             else if (photoStr.Replace("PHOTO;", "").StartsWith("GIF:"))
//             {
//                 photo.PhotoURL = photoStr.Replace("PHOTO;GIF:", "").Trim();
//                 photo.Encoding = PhotoEncoding.GIF;
//                 photo.Type = PhotoType.URL;
//                 photoCollection.Add(photo);
//             }
//         }
//
//         return photoCollection;
//     }
// }