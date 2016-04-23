using System;

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
    }
}