using System;

namespace vCardLib
{
    public class PhoneNumberCollection : System.Collections.CollectionBase
    {
        public void Add(PhoneNumber phoneNumber)
        {
            List.Add(phoneNumber);
        }

        public void Remove(PhoneNumber phoneNumber)
        {
            List.Remove(phoneNumber);
        }

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
