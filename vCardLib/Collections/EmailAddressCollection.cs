using System;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// Collection class to hold all contact emailAddresss
    /// </summary>
    public class EmailAddressCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add emailAddress to the emailAddress collection
        /// </summary>
        /// <param name="emailAddress">EmailAddress object to be added</param>
        public void Add(EmailAddress emailAddress)
        {
            List.Add(emailAddress);
        }

        /// <summary>
        /// Method to remove a emailAddress from the emailAddress collection
        /// </summary>
        /// <param name="emailAddress">EmailAddress object to be removed</param>
        public void Remove(EmailAddress emailAddress)
        {
            List.Remove(emailAddress);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a EmailAddress object at the specified index</returns>
        public EmailAddress this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (EmailAddress)List[index];
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
