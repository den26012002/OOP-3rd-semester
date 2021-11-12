using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class CreditsBankAccount : BaseBankAccount
    {
        private readonly int _minUnpayedCash = 0;
        internal CreditsBankAccount(uint id, Bank bankOwner, Client client, int startCash)
            : base(id, bankOwner, client, startCash)
        {
        }

        public override void WithdrawalCash(uint moneyToDraw)
        {
            var withdrawalCashCommand = new WithdrawalCashCommand(this, moneyToDraw);
            withdrawalCashCommand.Execute();

            CommandsHistory.Push(new Transaction(withdrawalCashCommand, ActualDateTime));
        }

        public override void DepositCash(uint moneyToDeposit)
        {
            var depositCashCommand = new DepositCashCommand(this, moneyToDeposit);
            depositCashCommand.Execute();
            CommandsHistory.Push(new Transaction(depositCashCommand, ActualDateTime));
        }

        public override void TransferCash(uint bankId, uint accountId, uint moneyToTransfer)
        {
            Bank targetBank = OwnerBank.CentralBank.GetBank(bankId);
            BaseBankAccount targetBankAccount = targetBank.GetBankAccount(accountId);

            var tranferCashCommand = new TransferCashCommand(this, targetBankAccount, moneyToTransfer);
            tranferCashCommand.Execute();

            CommandsHistory.Push(new Transaction(tranferCashCommand, ActualDateTime));
        }

        public override void UndoCash()
        {
            if (CommandsHistory.Count == 0)
            {
                throw new BanksException("Error: no transactions to undo");
            }

            Transaction latestTransaction = CommandsHistory.Peek();
            if (latestTransaction.TransactionDate != ActualDateTime)
            {
                throw new BanksException("Error: can't undo this transaction");
            }

            latestTransaction.CashCommand.Undo();
            CommandsHistory.Pop();
        }

        internal override void UpdateCountedFee(DateTime dateTime)
        {
            TimeSpan deltaTime = dateTime - ActualDateTime;
            if (Cash < _minUnpayedCash)
            {
                CountedFee -= deltaTime.Days * (int)OwnerBank.Conditions.CreditFee;
            }

            ActualDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        internal override void PayFee()
        {
            Cash += CountedFee;
            CountedFee = 0;
        }
    }
}
