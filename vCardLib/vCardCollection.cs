using System;

namespace vCardLib
{
    public class vCardCollection : System.Collections.CollectionBase
    {
        public void Add(vCard vcard)
        {
            List.Add(vcard);
        }

        public void Remove(vCard vcard)
        {
            List.Remove(vcard);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= List.Count)
                throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
            else
                List.RemoveAt(index);
        }

        public vCard this[int index]
        {
            get
            {
                if (index < 0 || index >= List.Count)
                    throw new IndexOutOfRangeException("Index cannot be " + index + " because collection does not contain as many elements");
                else
                    return (vCard)List[index];
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