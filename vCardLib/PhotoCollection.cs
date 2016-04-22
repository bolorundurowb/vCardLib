using System;

namespace vCardLib
{
    public class PhotoCollection : System.Collections.CollectionBase
    {
        public void Add(Photo photo)
        {
            List.Add(photo);
        }

        public void Remove(Photo photo)
        {
            List.Remove(photo);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= List.Count)
                throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
            else
                List.RemoveAt(index);
        }

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
