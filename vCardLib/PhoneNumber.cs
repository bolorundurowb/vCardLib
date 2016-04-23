using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vCardLib
{
    public class PhoneNumber
    {
        public string Number { get; set; }
        public PhoneNumberType Type { get; set; }
    }

    public enum PhoneNumberType
    {
        Work,
        Cell,
        Home,
        Voice
    }
}
