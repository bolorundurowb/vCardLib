using System;
using System.IO;
using System.Text;

namespace vCardLib.Utils
{
    /// <summary>
    /// Class to hold all supporting methods
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
            {
                throw new ArgumentNullException("The filepath supplied is null or empty");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file at the filepath does not exist");
            }

            var encoding = GetEncoding(filePath);
            var stream = new FileStream(filePath, FileMode.Open);
            return new StreamReader(stream, encoding);
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
            {
                throw new ArgumentNullException("The input stream reader cannot be null");
            }

            using (streamReader)
            {
                return streamReader.ReadToEnd();
            }
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
            {
                throw new ArgumentException("string cannot be null, empty or composed of only whitespace characters");
            }

            if (!(contactsString.Contains("BEGIN:VCARD") && contactsString.Contains("END:VCARD")))
            {
                throw new InvalidOperationException("The vcard file does not seem to be a valid vcard file");
            }

            contactsString = contactsString.Replace("BEGIN:VCARD", "");
            return contactsString.Split(new[] {"END:VCARD"}, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Sanitizes input and splits a single contact string intoits constituent parts
        /// </summary>
        /// <param name="contactString">A string containing the contact details</param>
        /// <returns>A string array of details, one per line</returns>
        public static string[] GetContactDetailsArrayFromString(string contactString)
        {
            contactString = contactString.Replace("PREF;", "").Replace("pref;", "");
            contactString = contactString.Replace("PREF,", "").Replace("pref,", "");
            contactString = contactString.Replace(",PREF", "").Replace(",pref", "");
            return contactString.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        public static Encoding GetEncoding(string filename)
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
