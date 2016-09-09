/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace vCardLib
{
    public enum PhotoEncoding
    {
        JPEG,
        GIF
    }

    public enum PhotoType
    {
        Image,
        URL
    }

    /// <summary>
    /// Class to hold images embedded in the vCard 
    /// </summary>
    public class Photo
    {
        public Bitmap Picture { get; set; }
        public PhotoType Type { get; set; }
        public PhotoEncoding Encoding { get; set; }
        public string PhotoURL { get; set; }

        public string ToBase64String()
        {
            if(Picture == null)
            {
                return "";
            }
            else
            {
                MemoryStream stream = new MemoryStream();
                Picture.Save(stream, ImageFormat.Bmp);
                byte[] imageBytes = stream.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
    }
}
