using System;
using System.Net.Mail;

namespace vCardLib
{
    public class EmailAddress
    {
        public MailAddress Email { get; set; }
        public EmailType Type { get; set; }
    }

    public enum EmailType
    {
        Work,
        Cell,
        Home
    }
}
