using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Class to hold email addresses
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        /// The email address
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The email address type
        /// </summary>
        public EmailAddressType Type { get; set; }

        /// <summary>
        /// Indicates the email address' preference level (lower  values mean a higher preference)
        /// </summary>
        public int? Preference { get; set; }
    }
}