using System;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// Collection class to hold all contact addresses
    /// </summary>
    public class AddressCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Add an address to the collection
        /// </summary>
        /// <param name="Address">An address</param>
        public void Add(Address Address)
        {
            List.Add(Address);
        }

        /// <summary>
        /// Method to remove an Address from the Address collection
        /// </summary>
        /// <param name="Address">Address object to be removed</param>
        public void Remove(Address Address)
        {
            List.Remove(Address);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>an Address object at the specified index</returns>
        public Address this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (Address)List[index];
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
