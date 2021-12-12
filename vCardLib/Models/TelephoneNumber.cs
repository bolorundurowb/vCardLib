using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Object to store phone  number details
    /// </summary>
    public class TelephoneNumber
    {
        /// <summary>
        /// The Number
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The telephone number extension
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// The number type
        /// </summary>
        public TelephoneNumberType Type { get; set; }

        /// <summary>
        /// Indicates the telephone numbers preference level (lower values mean a higher preference)
        /// </summary>
        public int? Preference { get; set; }
    }
}