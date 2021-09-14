namespace Shops.Entities
{
    public class Address
    {
        public Address(string street, uint houseNumber)
        {
            Street = street;
            HouseNumber = houseNumber;
        }

        public string Street { get; }
        public uint HouseNumber { get; }
    }
}
