using System;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// A collection of contacts' Hobbies
    /// </summary>
    public class HobbyCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add hobby to the hobby collection
        /// </summary>
        /// <param name="hobby">Hobby object to be added</param>
        public void Add(Hobby hobby)
        {
            List.Add(hobby);
        }

        /// <summary>
        /// Method to remove a hobby from the hobby collection
        /// </summary>
        /// <param name="hobby">Hobby object to be removed</param>
        public void Remove(Hobby hobby)
        {
            List.Remove(hobby);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a Hobby object at the specified index</returns>
        public Hobby this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                return (Hobby)List[index];
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
