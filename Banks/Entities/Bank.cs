using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class Bank
    {
        private int _nextAccountId = 0;
        private List<BaseBankAccount> _bankAccounts;

        internal Bank(int id, CentralBank centralBank, string name, BankConditions conditions = default)
        {
            Id = id;
            CentralBank = centralBank;
            Name = name;
            _bankAccounts = new List<BaseBankAccount>();
            EventManager = new BankEventManager();
            Conditions = conditions;
        }

        private Bank()
        {
        }

        public int Id { get; private init; }
        public CentralBank CentralBank { get; init; }
        public string Name { get; private init; }
        public IReadOnlyList<BaseBankAccount> BankAccounts => _bankAccounts;
        public BankEventManager EventManager { get; private init; }
        public BankConditions Conditions { get; private set; }

        public BaseBankAccount AddBankAccount(Client accountClient, AccountType accountType, int startCash, DateTime expirationDateTime = default)
        {
            BaseBankAccount newBankAccount = accountType switch
            {
                AccountType.Debit => new DebitsBankAccount(_nextAccountId++, this, accountClient, startCash),
                AccountType.Deposit => new DepositsBankAccount(_nextAccountId++, this, accountClient, startCash, expirationDateTime),
                AccountType.Credit => new CreditsBankAccount(_nextAccountId++, this, accountClient, startCash),
                _ => throw new BanksException("Error: unknown type of bank account"),
            };
            _bankAccounts.Add(newBankAccount);
            return newBankAccount;
        }

        public BaseBankAccount GetBankAccount(uint accountId)
        {
            BaseBankAccount bankAccount = _bankAccounts.Find(account => account.Id == accountId);
            if (bankAccount == null)
            {
                throw new BanksException($"Error: there're no bank account with Id {accountId}");
            }

            return bankAccount;
        }

        public bool IsClientShady(Client client)
        {
            return client.Address == null || client.PassportData == null;
        }

        public void UpdateConditions(BankConditions newConditions)
        {
            Conditions = newConditions;
            EventManager.Notify($"Bank: {Name}\n{Conditions.ToString()}");
        }

        internal void NotifyCountFee(DateTime dateTime)
        {
            foreach (BaseBankAccount bankAccount in _bankAccounts)
            {
                bankAccount.UpdateCountedFee(dateTime);
            }
        }

        internal void NotifyPayFee()
        {
            foreach (BaseBankAccount bankAccount in _bankAccounts)
            {
                bankAccount.PayFee();
            }
        }
    }
}
