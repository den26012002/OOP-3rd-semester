namespace Banks.Entities
{
    public class Client
    {
        internal Client(int id, string name, string surname, Address address = null, PassportData passportData = null)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Address = address;
            PassportData = passportData;
            BankEventListener = new BankEventListener();
        }

        private Client()
        {
        }

        public int Id { get; private init; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public Address Address { get; set; }
        public PassportData PassportData { get; set; }

        public BankEventListener BankEventListener { get; }
    }
}
