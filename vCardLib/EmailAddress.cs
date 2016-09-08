
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System.Net.Mail;

namespace vCardLib
{
    /// <summary>
    /// Class to hold email addresses
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        /// The email address
        /// </summary>
        public MailAddress Email { get; set; }
        /// <summary>
        /// The email address type
        /// </summary>
        public EmailType Type { get; set; }
    }

    /// <summary>
    /// Various email address types in a vCard
    /// </summary>
    public enum EmailType
    {
        Work,
        Internet,
        Home,
        AOL,
        Applelink,
        IBMMail,
        None
    }
}
