namespace vCardLib
{
    public class Address
    {
        public string Location { get; set; }
        public AddressType Type { get; set; }
    }

    public enum AddressType
    {
        Work,
        Home,
        Domestic,
        International,
        Postal,
        Parcel,
        None
    }
}
