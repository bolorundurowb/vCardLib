
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System.Drawing;

namespace vCardLib
{
    /// <summary>
    /// Class to hold images embedded in the vCard 
    /// </summary>
    public class Photo
    {
        public Bitmap Picture { get; set; }
        public PhotoType Type { get; set; }
        public PhotoEncoding Encoding { get; set; }
        public string PhotoURL { get; set; }
    }

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
}
