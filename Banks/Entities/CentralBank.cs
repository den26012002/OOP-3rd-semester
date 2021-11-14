using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBank
    {
        private readonly int _payingFeesNotificationDay = 1;
        private int _nextBankId = 0;
        private List<Bank> _banks;
        private Time _time;

        public CentralBank(DateTime startDateTime = default)
        {
            _banks = new List<Bank>();
            _time = new Time(startDateTime);
            CentralClientsRegistrator = new ClientsRegistrator();
        }

        private CentralBank()
        {
        }

        public int Id { get; private init; }
        public IReadOnlyList<Bank> Banks => _banks;
        public ClientsRegistrator CentralClientsRegistrator { get; private set; }

        public Bank AddBank(string bankName, BankConditions conditions = default)
        {
            var newBank = new Bank(_nextBankId++, this, bankName, conditions);
            _banks.Add(newBank);
            return newBank;
        }

        public Bank GetBank(uint bankId)
        {
            Bank bank = _banks.Find(bank => bank.Id == bankId);
            if (bank == null)
            {
                throw new BanksException($"Error: there're no bank with Id {bankId}");
            }

            return bank;
        }

        public void ScrollTime(int numberOfDays)
        {
            while (numberOfDays > 0)
            {
                _time.Scroll(1);
                NotifyCountFees(_time.Now);
                if (_time.Now.Day == _payingFeesNotificationDay)
                {
                    NotifyPayFees();
                }

                --numberOfDays;
            }
        }

        public DateTime GetDateNow()
        {
            return _time.Now;
        }

        internal void UpdateBanksClients(List<Bank> banks, List<Client> clients)
        {
            _banks = banks;
            CentralClientsRegistrator = new ClientsRegistrator();
            foreach (Client client in clients)
            {
                CentralClientsRegistrator.RegisterClient(client.Name, client.Surname, client.Address, client.PassportData);
            }
        }

        private void NotifyCountFees(DateTime dateTime)
        {
            foreach (Bank bank in _banks)
            {
                bank.NotifyCountFee(dateTime);
            }
        }

        private void NotifyPayFees()
        {
            foreach (Bank bank in _banks)
            {
                bank.NotifyPayFee();
            }
        }
    }
}
