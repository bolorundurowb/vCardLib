using System;
using System.IO;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class vCardExtensions
    {
        public static void Save(this vCard card, string path, Encoding encoding = null, vCardVersion? version = null, OverWriteOptions overWriteOptions = OverWriteOptions.Proceed)
        {
            var contents = card.ToString(version ?? card.Version);

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
