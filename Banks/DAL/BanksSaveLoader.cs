using System.Linq;
using Banks.Entities;

namespace Banks.DAL
{
    public class BanksSaveLoader
    {
        private CentralBank _centralBank;

        public BanksSaveLoader(CentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public void Save(BanksContext banksContext)
        {
            banksContext.Database.EnsureDeleted();
            banksContext.Database.EnsureCreated();
            foreach (Bank bank in _centralBank.Banks)
            {
                banksContext.Banks.Add(bank);
            }

            foreach (Client client in _centralBank.CentralClientsRegistrator.Clients)
            {
                banksContext.Clients.Add(client);
            }

            banksContext.SaveChanges();
        }

        public void Load(BanksContext banksContext)
        {
            var banks = banksContext.Banks.ToList();
            var clients = banksContext.Clients.ToList();
            banksContext.BankAccounts.ToList();
            banksContext.BankEventListeners.ToList();
            banksContext.BankEventManagers.ToList();
            banksContext.BanksConditions.ToList();
            banksContext.CashCommands.ToList();
            banksContext.Transactions.ToList();
            _centralBank.UpdateBanksClients(banks, clients);
            /*foreach (Bank bank in banks)
            {
                Bank newBank = _centralBank.AddBank(bank.Name, bank.Conditions);
                foreach (BaseBankAccount bankAccount in bank.BankAccounts)
                {
                    if (bankAccount is DebitsBankAccount debitsBankAccount)
                    {
                        newBank.AddBankAccount(debitsBankAccount.AccountClient, AccountType.Debit, )
                    }
                }
            }*/
        }
    }
}
