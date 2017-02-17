using System;
using System.IO;
using vCardLib.Helpers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Collections
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
                return (vCard)List[index];
            }
            set
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                List[index] = value;
            }
        }

        /// <summary>
        /// Saves a vcard collection to a vcf file
        /// </summary>
        /// <param name="filePath">Path to save to</param>
        /// <param name="writeOptions">Determines if the method throws and exception if the save path exists or not</param>
        /// <exception cref="InvalidOperationException">The file already exists</exception>
        /// <exception cref="NotImplementedException">version 4 files are not yet supported</exception>
        /// <exception cref="ArgumentException">The vcard version is invalid</exception>
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
                if(vcard.Version == Version.V2)
                {
                    vcard.WriteV2ObjectToString(ref vcardString);
                }
                else if (vcard.Version == Version.V3)
                {
                    vcard.WriteV3ObjectToString(ref vcardString);
                }
				else if (vcard.Version == Version.V4)
				{
					throw new NotImplementedException("Writing for v4 is not implemented");
				}
				else
				{
					throw new ArgumentException("version is not a valid vcf version");
				}
            }
            File.WriteAllText(filePath, vcardString);
        }

        /// <summary>
        /// Saves a vcard collection to a vcf file
        /// </summary>
        /// <param name="filePath">Path to save to</param>
        /// <param name="version">Specify the version you want to save in</param>
        /// <param name="writeOptions">Determines if the method throws and exception if the save path exists or not</param>
        /// <exception cref="InvalidOperationException">The file already exists</exception>
        /// <exception cref="NotImplementedException">version 4 files are not yet supported</exception>
        /// <exception cref="ArgumentException">The vcard version is invalid</exception>
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
			else if (version == 4.0F)
			{
				throw new NotImplementedException("Writing for v4 is not implemented");
			}
			else
			{
				throw new ArgumentException("version is not a valid vcf version");
			}
            File.WriteAllText(filePath, vcardString);
        }
    }
}
