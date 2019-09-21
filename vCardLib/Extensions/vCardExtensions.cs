using System;
using System.IO;
using System.Text;
using vCardLib.Helpers;
using vCardLib.Serializers;

namespace vCardLib.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class vCardExtensions
    {
        public static void Save(this vCard This, string path, Encoding encoding = null, vCardVersion? version = null, OverWriteOptions overWriteOptions = OverWriteOptions.Proceed)
        {
            Serializer serializer;

            switch (version ?? This.Version)
            {
                case vCardVersion.V2:
                    serializer = new V2Serializer();
                    break;
                case vCardVersion.V3:
                    serializer = new V3Serializer();
                    break;
                default:
                    serializer = new V4Serializer();
                    break;
            }

            var contents = serializer.Serialize(This);

            if (File.Exists(path) && overWriteOptions == OverWriteOptions.Throw)
            {
                throw new InvalidOperationException("A file already exists at the provided path.");
            }

            File.WriteAllText(path, contents, encoding ?? Encoding.UTF8);
        }
    }
}
