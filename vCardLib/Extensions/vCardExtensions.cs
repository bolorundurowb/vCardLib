using System;
using System.IO;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class vCardExtensions
    {
        private static readonly V2Serializer _v2Serializer = new V2Serializer();
        private static readonly V3Serializer _v3Serializer = new V3Serializer();
        private static readonly V4Serializer _v4Serializer = new V4Serializer();
        
        public static void Save(this vCard This, string path, Encoding encoding = null, vCardVersion? version = null, OverWriteOptions overWriteOptions = OverWriteOptions.Proceed)
        {
            string contents;

            switch (version ?? This.Version)
            {
                case vCardVersion.V2:
                    contents = _v2Serializer.Serialize(This);
                    break;
                case vCardVersion.V3:
                    contents = _v3Serializer.Serialize(This);
                    break;
                default:
                    contents = _v4Serializer.Serialize(This);
                    break;
            }

            if (File.Exists(path) && overWriteOptions == OverWriteOptions.Throw)
            {
                throw new InvalidOperationException(
                    "A file with the given filePath exists."
                    + " If you want to overwrite the file,"
                    + " then call this method and pass the "
                    + "optional overwrite option"
                );
            }

            File.WriteAllText(path, contents, encoding ?? Encoding.UTF8);
        }
    }
}
