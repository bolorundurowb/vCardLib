using System;
using System.Drawing;
using System.IO;

namespace vCardLib.Helpers
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
            {
                throw new ArgumentNullException("The input stream reader cannot be null");
            }
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
            {
                throw new ArgumentException("string cannot be null, empty or composed of only whitespace characters");
            }
            if (!(contactsString.Contains("BEGIN:VCARD") && contactsString.Contains("END:VCARD")))
            {
                throw new InvalidOperationException("The vcard file does not seem to be a valid vcard file");
            }
            contactsString = contactsString.Replace("BEGIN:VCARD", "");
            return contactsString.Split(new[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries);
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
            return contactString.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Converts a base 64 string to an image
        /// </summary>
        /// <param name="base64String">properly encoded base 64 string</param>
        /// <returns>A bitmap object or null if the string is invalid</returns>
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
