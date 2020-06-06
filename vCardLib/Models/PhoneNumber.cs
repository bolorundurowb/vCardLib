using vCardLib.Enums;

namespace vCardLib.Models
{
    /// <summary>
    /// Object to store phone  number details
    /// </summary>
    public class PhoneNumber
    {
        /// <summary>
        /// The Number
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// The number type
        /// </summary>
        public PhoneNumberType Type { get; set; }
    }
}
