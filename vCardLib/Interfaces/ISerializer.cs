using System.Collections.Generic;
using vCardLib.Models;

namespace vCardLib.Interfaces
{
    public interface ISerializer
    {
        string AddPhoneNumbers(IEnumerable<PhoneNumber> phoneNumbers);

        string AddEmailAddresses(IEnumerable<EmailAddress> emailAddresses);

        string AddAddresses(IEnumerable<Address> addresses);

        string AddPhotos(IEnumerable<Photo> photos);

        string AddExpertises(IEnumerable<Expertise> expertises);

        string AddHobbies(IEnumerable<Hobby> hobbies);

        string AddInterests(IEnumerable<Interest> interests);
    }
}