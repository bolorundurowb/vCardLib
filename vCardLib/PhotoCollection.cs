using System;

namespace vCardLib
{
    /// <summary>
    /// Collection class to hold all contact photos
    /// </summary>
    public class PhotoCollection : System.Collections.CollectionBase
    {
        /// <summary>
        /// Method to add photo to the photo collection
        /// </summary>
        /// <param name="photo">Photo object to be added</param>
        public void Add(Photo photo)
        {
            List.Add(photo);
        }

        /// <summary>
        /// Method to remove a photo from the photo collection
        /// </summary>
        /// <param name="photo">Photo object to be removed</param>
        public void Remove(Photo photo)
        {
            List.Remove(photo);
        }

        /// <summary>
        /// Indexer to enable index access to the collection
        /// </summary>
        /// <param name="index">zero based index of the object to be returned</param>
        /// <returns>a Photo object at the specified index</returns>
        public Photo this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (Photo)List[index];
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
