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
            /*            var list = new List<DepositFee>();
                        list.Add(new DepositFee(1, 10));
                        list.Add(new DepositFee(2, 20));
                        list.Add(new DepositFee(3, 100));
                        var conditions = new BankConditions(1, list, 10);
                        Console.WriteLine(conditions.ToString());*/
            /*
                        var dateTime = new DateTime(10, 1, 30, 1, 5, 10);
                        dateTime = dateTime.AddDays(10);
                        Console.WriteLine(dateTime.Date);*/
            /*            var client = new Client("soadijf", "jfioew");
                        var bankAccount = new CreditsBankAccount(client, -10);
                        var date1 = new DateTime(1, 2, 3);
                        var date2 = new DateTime(10, 5, 30);
                        Console.WriteLine(date1);
                        Console.WriteLine(date2);*/
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
