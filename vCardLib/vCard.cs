using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vCardLib
{
    public class vCard
    {
        public static vCardCollection FromFile(string filepath)
        {
            string vcfString;
            using(StreamReader streamReader = new StreamReader(filepath))
            {
                vcfString = streamReader.ReadToEnd();
            }
            vcfString = vcfString.Replace("BEGIN:VCARD", "");
            string[] contactStrings = vcfString.Split(new string[] { "END:VCARD" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
