
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System;
using System.IO;

namespace vCardLib
{
    /// <summary>
    /// Collection class to hold all vCard objects extracted
    /// </summary>
    public class vCardCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add a new vCard to the collection
        /// </summary>
        /// <param name="vcard">vCard object to be added</param>
        public void Add(vCard vcard)
        {
            List.Add(vcard);
        }

        /// <summary>
        /// Method to remove a new vCard from the collection
        /// </summary>
        /// <param name="vcard">vCard object to be removed</param>
        public void Remove(vCard vcard)
        {
            List.Remove(vcard);
        }

        /// <summary>
        /// Indexer to enable index access to the vCard collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a vCard object at the specified index</returns>
        public vCard this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (vCard)List[index];
            }
            set
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    List[index] = value;
            }
        }

        public void Save(string filePath, WriteOptions writeOptions = WriteOptions.ThrowError)
        {
            if (writeOptions == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException("A file with the given filePath exists. If you want to overwrite the file, then call this method and pass the optional overwrite option");
                }
            }
            string vcardString = "";
            foreach(vCard vcard in this)
            {
                if(vcard.Version == 2.1f)
                {
                    vcard.WriteV2ObjectToString(ref vcardString);
                }
                else if (vcard.Version == 3.0f)
                {
                    vcard.WriteV3ObjectToString(ref vcardString);
                }
            }
            File.WriteAllText(filePath, vcardString);
        }

        public void Save(string filePath, float version, WriteOptions writeOptions = WriteOptions.ThrowError)
        {
            if (writeOptions == WriteOptions.ThrowError)
            {
                if (File.Exists(filePath))
                {
                    throw new InvalidOperationException("A file with the given filePath exists. If you want to overwrite the file, then call this method and pass the optional overwrite option");
                }
            }
            string vcardString = "";

            if (version == 2.1f)
            {
                foreach (vCard vcard in this)
                {
                    vcard.WriteV2ObjectToString(ref vcardString);
                }
            }
            else if (version == 3.0f)
            {
                foreach (vCard vcard in this)
                {
                    vcard.WriteV3ObjectToString(ref vcardString);
                }
            }
            File.WriteAllText(filePath, vcardString);
        }
    }
}