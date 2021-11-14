using Banks.DAL;
using Banks.Entities;
using Banks.UI;
using Microsoft.EntityFrameworkCore;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var centralBank = new CentralBank();
            var banksUIContext = new BanksUIContext(centralBank);
            var banksSaveLoader = new BanksSaveLoader(centralBank);
            Bank bank = centralBank.AddBank("bankName");
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            bank.AddBankAccount(client, AccountType.Debit, 10);
            var optionsBuilder = new DbContextOptionsBuilder<BanksContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=banksdb;Trusted_Connection=True;");
            using (var banksContext = new BanksContext(optionsBuilder.Options))
            {
                banksSaveLoader.Save(banksContext);
            }

            var anotherCentralBank = new CentralBank();
            var anotherBanksSaveLoader = new BanksSaveLoader(anotherCentralBank);
            using (var banksContext = new BanksContext(optionsBuilder.Options))
            {
                anotherBanksSaveLoader.Load(banksContext);
            }

            banksUIContext.Show();
        }
    }
}
