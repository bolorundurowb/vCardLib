
/* =======================================================================
 * vCard Library for .NET
 * Copyright (c) 2016 Bolorunduro Winner-Timothy http://www.github.com/VCF-Reader
 * .
 * ======================================================================= */

using System;

namespace vCardLib
{
    /// <summary>
    /// Collection class to hold all contact phone numbers
    /// </summary>
    public class PhoneNumberCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add phone number to the phone number collection
        /// </summary>
        /// <param name="phoneNumber">Phone number object to be added</param>
        public void Add(PhoneNumber phoneNumber)
        {
            List.Add(phoneNumber);
        }

        /// <summary>
        /// Method to remove a phone number from the phone number collection
        /// </summary>
        /// <param name="phoneNumber">Phone number object to be removed</param>
        public void Remove(PhoneNumber phoneNumber)
        {
            List.Remove(phoneNumber);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a Phone number object at the specified index</returns>
        public PhoneNumber this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (PhoneNumber)List[index];
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
