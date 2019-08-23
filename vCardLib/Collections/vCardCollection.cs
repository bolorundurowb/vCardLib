using System;
using System.Text;
using vCardLib.Helpers;
using vCardLib.Serializers;
using Version = vCardLib.Helpers.Version;

namespace vCardLib.Collections
{
    /// <summary>
    /// Collection class to hold all vCard objects extracted
    /// </summary>
    // ReSharper disable once InconsistentNaming
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
                {
                    throw new IndexOutOfRangeException("Index cannot be " + index +
                                                       " because collection does not contain as many elements");
                }

                return (vCard) List[index];
            }
            set
            {
                if (index < 0 || index >= List.Count)
                {
                    throw new IndexOutOfRangeException("Index cannot be " + index +
                                                       " because collection does not contain as many elements");
                }

                List[index] = value;
            }
        }

        /// <summary>
        /// Saves a vcard collection to a vcf file
        /// </summary>
        /// <param name="filePath">Path to save to</param>
        /// <param name="version">Specify the version you want to save in</param>
        /// <param name="encoding">The file encoding to use</param>
        /// <param name="writeOptions">Determines if the method throws and exception if the save path exists or not</param>
        /// <exception cref="InvalidOperationException">The file already exists</exception>
        /// <exception cref="NotImplementedException">version 4 files are not yet supported</exception>
        /// <exception cref="ArgumentException">The vcard version is invalid</exception>
        public bool Save(string filePath, Version version, WriteOptions writeOptions = WriteOptions.ThrowError,
            Encoding encoding = null)
        {
            return Serializer.Serialize(this, filePath, version, writeOptions, encoding);
        }
    }
}
