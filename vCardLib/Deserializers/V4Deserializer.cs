using System;
using vCardLib.Models;

namespace vCardLib.Deserializers
{
    public static class V4Deserializer
    {
        /// <summary>
        /// Parse the text representing the vCard object
        /// </summary>
        /// <param name="contactDetailStrings">An array of the vcard properties as strings</param>
        /// <param name="vcard">A partial vcard</param>
        /// <returns>A version 4 vcard object</returns>
        /// <exception cref="NotImplementedException">This method is not implemented yet</exception>
        public static vCard Parse(string[] contactDetailStrings, vCard vcard)
        {
            throw new NotImplementedException("Sorry, support for vcard 4.0 hasn't been implemented");
        }
    }
}
