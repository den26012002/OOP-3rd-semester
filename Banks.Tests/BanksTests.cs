using System.Collections.Generic;
using Banks.Entities;
using Banks.Tools;
using NUnit.Framework;

namespace Banks.Tests
{
    class Tests
    {
        [Test]
        public void ShadyClientWithdrawalUpperLimit_ThrowException()
        {
            Assert.Catch<BanksException>(() =>
            {
                var centralBank = new CentralBank();
                var depositFees = new List<DepositFee>();
                var bankConditions = new BankConditions(10, depositFees, 10);
                Bank bank = centralBank.AddBank("bank", bankConditions);
                Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
                BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Debit, 100);
                bankAccount.WithdrawalCash(11);
            });
        }

        [Test]
        public void ShadyClientBecameUnshadyAndWithdrawalUpperLimit_MoneyWithdrawed()
        {
            var centralBank = new CentralBank();
            var depositFees = new List<DepositFee>();
            var bankConditions = new BankConditions(10, depositFees, 10);
            Bank bank = centralBank.AddBank("bank", bankConditions);
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            client.Address = new Address("street", 10);
            client.PassportData = new PassportData(10, 10);
            int startSum = 100;
            BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Debit, startSum);
            uint sumToWithdrawal = 11;
            bankAccount.WithdrawalCash(sumToWithdrawal);
            if (bankAccount.Cash != startSum - sumToWithdrawal)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void DepositAccountWithdrawalMoney_ThrowException()
        {
            Assert.Catch<BanksException>(() =>
            {
                var centralBank = new CentralBank();
                var depositFees = new List<DepositFee>();
                var bankConditions = new BankConditions(10, depositFees, 10);
                Bank bank = centralBank.AddBank("bank", bankConditions);
                Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
                BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Deposit, 100, new System.DateTime(10, 10, 10));
                bankAccount.WithdrawalCash(1);
            });
        }

        [Test]
        public void DepositAccountsOpened_AfterMonthPercentsPaid()
        {
            var centralBank = new CentralBank();
            var depositFees = new List<DepositFee>();
            depositFees.Add(new DepositFee(10, 1));
            var bankConditions = new BankConditions(10, depositFees, 10);
            Bank bank = centralBank.AddBank("bank", bankConditions);
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            int startCash1 = 100;
            BaseBankAccount bankAccount1 = bank.AddBankAccount(client, AccountType.Deposit, startCash1, new System.DateTime(10, 10, 10));
            int startCash2 = 9;
            BaseBankAccount bankAccount2 = bank.AddBankAccount(client, AccountType.Deposit, startCash2, new System.DateTime(10, 10, 10));
            centralBank.ScrollTime(31);
            if (bankAccount2.Cash != startCash2)
            {
                Assert.Fail();
            }
            if (bankAccount1.Cash <= startCash1)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void DepositAccountOpened_AfterLessThanMonthPercentsNotPaid()
        {
            var centralBank = new CentralBank();
            var depositFees = new List<DepositFee>();
            depositFees.Add(new DepositFee(10, 1));
            var bankConditions = new BankConditions(10, depositFees, 10);
            Bank bank = centralBank.AddBank("bank", bankConditions);
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            int startCash = 100;
            BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Deposit, startCash, new System.DateTime(10, 10, 10));
            centralBank.ScrollTime(3);

            if (bankAccount.Cash != startCash)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CreditAccountOpenedLessZeroMoney_AfterMonthFeePaid()
        {
            var centralBank = new CentralBank();
            var depositFees = new List<DepositFee>();
            depositFees.Add(new DepositFee(10, 1));
            var bankConditions = new BankConditions(10, depositFees, 10);
            Bank bank = centralBank.AddBank("bank", bankConditions);
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            int startCash = -100;
            BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Credit, startCash);
            centralBank.ScrollTime(31);

            if (bankAccount.Cash >= startCash)
            {
                Assert.Fail();
            }
        }

        [Test]
        public void CreditAccountOpenedMoreZeroMoney_AfterMonthFeeNodPaid()
        {
            var centralBank = new CentralBank();
            var depositFees = new List<DepositFee>();
            depositFees.Add(new DepositFee(10, 1));
            var bankConditions = new BankConditions(10, depositFees, 10);
            Bank bank = centralBank.AddBank("bank", bankConditions);
            Client client = centralBank.CentralClientsRegistrator.RegisterClient("name", "surname");
            int startCash = 100;
            BaseBankAccount bankAccount = bank.AddBankAccount(client, AccountType.Credit, startCash);
            centralBank.ScrollTime(31);

            if (bankAccount.Cash != startCash)
            {
                Assert.Fail();
            }
        }
    }
}
