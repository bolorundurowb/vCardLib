using System;
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
    }
}
