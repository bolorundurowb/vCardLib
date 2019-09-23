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
        public string Email { get; set; }
        
        /// <summary>
        /// The email address type
        /// </summary>
        public EmailType Type { get; set; }
    }
}
