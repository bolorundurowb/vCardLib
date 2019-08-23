using System;
using System.Collections;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// Collection class to hold all contact addresses
    /// </summary>
    public class AddressCollection : CollectionBase
    {
        /// <summary>
        /// Add an address to the collection
        /// </summary>
        /// <param name="address">An address</param>
        public void Add(Address address)
        {
            List.Add(address);
        }

        /// <summary>
        /// Method to remove an Address from the Address collection
        /// </summary>
        /// <param name="address">Address object to be removed</param>
        public void Remove(Address address)
        {
            List.Remove(address);
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
                {
                    throw new IndexOutOfRangeException("Index cannot be " + index +
                                                       " because collection does not contain as many elements");
                }

                return (Address) List[index];
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
    }
}
