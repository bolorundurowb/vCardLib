using System;

namespace vCardLib.Serializers
{
    /// <summary>
    /// Handles the serialization of version 4 cards
    /// </summary>
    public class V4Serializer
    {
        /// <summary>
        /// Converts the vCard properties to a string
        /// </summary>
        /// <param name="vcard">The vcard object to be serialized</param>
        /// <returns>A string representing the vcard properties</returns>
        /// <exception cref="NotImplementedException">This method hasn't been implemented</exception>
        public static string Serialize(vCard vcard)
        {
            throw new NotImplementedException("Writing for v4 is not implemented");
        }
    }
}
