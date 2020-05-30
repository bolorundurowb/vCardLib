using System.Collections.Generic;
using System.Text;
using vCardLib.Enums;
using vCardLib.Models;

namespace vCardLib.Interfaces
{
    public interface ISerializer
    {
        string Serialize(vCard vCard);
        
        string Serialize(IEnumerable<vCard> vCardCollection, vCardVersion? version = null);
        
        void AddPhoneNumbers(StringBuilder stringBuilder, IEnumerable<PhoneNumber> phoneNumbers);

        void AddEmailAddresses(StringBuilder stringBuilder, IEnumerable<EmailAddress> emailAddresses);

        void AddAddresses(StringBuilder stringBuilder, IEnumerable<Address> addresses);

        void AddPhotos(StringBuilder stringBuilder, IEnumerable<Photo> photos);

        void AddExpertises(StringBuilder stringBuilder, IEnumerable<Expertise> expertises);

        void AddHobbies(StringBuilder stringBuilder, IEnumerable<Hobby> hobbies);

        void AddInterests(StringBuilder stringBuilder, IEnumerable<Interest> interests);
    }
}