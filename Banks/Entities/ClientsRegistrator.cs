using System.Collections.Generic;

namespace Banks.Entities
{
    public class ClientsRegistrator
    {
        private uint nextClientId;
        private List<Client> _clients;

        public ClientsRegistrator()
        {
            _clients = new List<Client>();
        }

        public IReadOnlyList<Client> Clients => _clients;

        public Client RegisterClient(string name, string surname, Address address = default, PassportData passportData = default)
        {
            var newClient = new Client(nextClientId++, name, surname, address, passportData);
            _clients.Add(newClient);
            return newClient;
        }
    }
}
