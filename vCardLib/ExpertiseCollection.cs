using System;

namespace vCardLib
{
    public class ExpertiseCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add expertise to the expertise collection
        /// </summary>
        /// <param name="expertise">Expertise object to be added</param>
        public void Add(Expertise expertise)
        {
            List.Add(expertise);
        }

        /// <summary>
        /// Method to remove a expertise from the expertise collection
        /// </summary>
        /// <param name="expertise">Expertise object to be removed</param>
        public void Remove(Expertise expertise)
        {
            List.Remove(expertise);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a Expertise object at the specified index</returns>
        public Expertise this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (Expertise)List[index];
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
