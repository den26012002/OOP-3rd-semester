namespace Banks.Entities
{
    public class Address
    {
        public Address(string street, uint houseNumber)
        {
            Street = street;
            HouseNumber = houseNumber;
        }

        public int Id { get; private init; }
        public string Street { get; private init; }
        public uint HouseNumber { get; private init; }
    }
}
