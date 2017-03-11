using System;
using System.IO;
using vCardLib.Collections;
using vCardLib.Helpers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Serializers
{
    public class Serializer
    {
        public static bool Serialize(vCard vcard, string filePath, Version version, WriteOptions options = WriteOptions.ThrowError)
        {
            if (options == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException(
                        "A file with the given filePath exists."
                        + " If you want to overwrite the file,"
                        + " then call this method and pass the "
                        + "optional overwrite option"
                    );
                }
            }
            if (vcard == null)
            {
                throw new ArgumentNullException("The vcard cannot be null.");
            }
            if (version == Version.V2)
            {
                try
                {
                    string vcfString = V2Serializer.Serialize(vcard);
                    File.WriteAllText(filePath, vcfString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else if (version == Version.V3)
            {
                try
                {
                    string vcfString = V3Serializer.Serialize(vcard);
                    File.WriteAllText(filePath, vcfString);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
            else if (version == Version.V4)
            {
                V4Serializer.Serialize(vcard);
            }
            return true;
        }

        public static bool Serialize(vCardCollection vcardCollection, string filePath, Version version,
            WriteOptions options = WriteOptions.ThrowError)
        {
            if (options == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException(
                        "A file with the given filePath exists."
                        + " If you want to overwrite the file,"
                        + " then call this method and pass the "
                        + "optional overwrite option"
                    );
                }
            }
            var vcardString = "";
            if (version == Version.V2)
            {
                foreach(vCard vcard in vcardCollection)
                {
                    vcardString += V2Serializer.Serialize(vcard);
                }
            }
            else if (version == Version.V3)
            {
                foreach (vCard vcard in vcardCollection)
                {
                    vcardString += V3Serializer.Serialize(vcard);
                }
            }
            else
            {
                foreach (vCard vcard in vcardCollection)
                {
                    vcardString += V4Serializer.Serialize(vcard);
                }
            }
            try
            {
                File.WriteAllText(filePath, vcardString);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}
