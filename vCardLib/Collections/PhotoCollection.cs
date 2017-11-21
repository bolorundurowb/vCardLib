﻿using System;
using vCardLib.Models;

namespace vCardLib.Collections
{
    /// <summary>
    /// Collection class to hold all contact photos
    /// </summary>
    public sealed class PhotoCollection : System.Collections.CollectionBase, IDisposable
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
                return (Photo)List[index];
            }
            set
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                List[index] = value;
            }
        }

		#region IDisposable Support
		public void Dispose()
		{
			foreach ( Photo photo in List )
			{
				photo.Dispose();
			}

		}
		#endregion
	}
}
