using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace vCardLib.Utils
{
    /// <summary>
    /// Class to hold all supporting methods
    /// </summary>
    public class Helpers
    {
        public static string[][] GetContactsFromFile(string filePath)
        {
            var stream = new FileStream(filePath, FileMode.Open);
            return GetContactsFromStream(stream);
        }

        public static string[][] GetContactsFromStream(Stream stream)
        {
            var encoding = GetEncoding(stream);
            using (var reader = new StreamReader(stream, encoding))
            {
                var contents = reader.ReadToEnd();
                var response = new List<string[]>();
                var contacts = GetIndividualContacts(contents);
                foreach (var contact in contacts)
                {
                    response.Add(ExtractContactDetails(contact));
                }

                return response
                    .Where(x => x.Length > 0)
                    .ToArray();
            }
        }

        /// <summary>
        /// Splits a single contact string into individual contact strings
        /// </summary>
        /// <param name="allContacts">A string representation of the vcard</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">The input is null or empty</exception>
        /// <exception cref="InvalidOperationException">The string does not start and end with appropriate tags</exception>
        private static string[] GetIndividualContacts(string allContacts)
        {
            if (string.IsNullOrWhiteSpace(allContacts))
            {
                return new string[0];
            }

            if (!allContacts.Contains(Constants.StartToken) || !allContacts.Contains(Constants.EndToken))
            {
                throw new InvalidOperationException("The vCard contents are invalid.");
            }

            return allContacts.Split(new[]
            {
                Constants.EndToken
            }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Sanitizes input and splits a single contact string intoits constituent parts
        /// </summary>
        /// <param name="contact">A string containing the contact details</param>
        /// <returns>A string array of details, one per line</returns>
        private static string[] ExtractContactDetails(string contact)
        {
            var normalizedContact = contact
                .Replace("PREF;", "")
                .Replace("pref;", "")
                .Replace("PREF,", "")
                .Replace("pref,", "")
                .Replace(",PREF", "")
                .Replace(",pref", "");

            return normalizedContact.Split(new[]
            {
                "\n", "\r\n"
            }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        private static Encoding GetEncoding(Stream stream)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                return Encoding.UTF8;
            }

            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode;
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode;
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;

            return Encoding.ASCII;
        }
    }
}