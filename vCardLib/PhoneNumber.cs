
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

namespace vCardLib
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

    /// <summary>
    /// Various types of numbers stored in a vcard
    /// </summary>
    public enum PhoneNumberType
    {
        Work,
        Cell,
        Home,
        Voice
    }
}
