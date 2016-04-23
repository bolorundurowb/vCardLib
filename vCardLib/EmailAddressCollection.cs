using System;

namespace vCardLib
{
    public class EmailAddressCollection : System.Collections.CollectionBase
    {
        public void Add(EmailAddress emailAddress)
        {
            List.Add(emailAddress);
        }

        public void Remove(EmailAddress emailAddress)
        {
            List.Remove(emailAddress);
        }

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
