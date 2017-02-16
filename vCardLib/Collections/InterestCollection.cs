using System;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// A collection to hold a contacts' interests
    /// </summary>
    public class InterestCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add interest to the interest collection
        /// </summary>
        /// <param name="interest">Interest object to be added</param>
        public void Add(Interest interest)
        {
            List.Add(interest);
        }

        /// <summary>
        /// Method to remove a interest from the interest collection
        /// </summary>
        /// <param name="interest">Interest object to be removed</param>
        public void Remove(Interest interest)
        {
            List.Remove(interest);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a Interest object at the specified index</returns>
        public Interest this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                return (Interest)List[index];
            }
            set
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                List[index] = value;
            }
        }
    }
}
