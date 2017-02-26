using System;

namespace vCardLib.Deserializers
{
    public class V4Deserializer
    {
        /// <summary>
        /// Parse the text representing the vCard object
        /// </summary>
        /// <param name="contactDetails"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static vCard Parse(string[] contactDetails)
        {
            throw new NotImplementedException("Sorry, support for vcard 4.0 hasn't been implemented");
        }
    }
}
