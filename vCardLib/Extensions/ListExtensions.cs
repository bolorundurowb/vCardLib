using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;
using vCardLib.Serializers;

namespace vCardLib.Extensions
{
    public static class ListExtensions
    {
        public static void Save(this List<vCard> This, string path, Encoding encoding = null, vCardVersion? version = null, OverWriteOptions overWriteOptions = OverWriteOptions.Proceed)
        {
            var stringBuilder = new StringBuilder();
            if (File.Exists(path) && overWriteOptions == OverWriteOptions.Throw)
            {
                throw new InvalidOperationException(
                    "A file with the given filePath exists."
                    + " If you want to overwrite the file,"
                    + " then call this method and pass the "
                    + "optional overwrite option"
                );
            }

            if (!This.Any())
            {
                File.WriteAllText(path, String.Empty, encoding ?? Encoding.UTF8);
            }
            else
            {
                var selectedVersion = version ?? This.First().Version;
                Serializer serializer;

                switch (selectedVersion)
                {
                    case vCardVersion.V2:
                        serializer = new V2Serializer();
                        break;
                    case vCardVersion.V3:
                        serializer = new V3Serializer();
                        break;
                    case vCardVersion.V4:
                        serializer = new V4Serializer();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                foreach (var vCard in This)
                {
                    stringBuilder.Append(serializer.Serialize(vCard)).AppendLine();
                }

                var contents = stringBuilder.ToString();
                File.WriteAllText(path, contents, encoding ?? Encoding.UTF8);
            }
        }
    }
}
