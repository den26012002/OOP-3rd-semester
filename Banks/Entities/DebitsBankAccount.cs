using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DebitsBankAccount : BaseBankAccount
    {
        private readonly int _minAllowedCash = 0;

        internal DebitsBankAccount(int id, Bank bankOwner, Client client, int startCash)
            : base(id, bankOwner, client, startCash)
        {
            if (startCash < 0)
            {
                throw new BanksException("Error: money on debits account can't be less than 0");
            }
        }

        private DebitsBankAccount()
        {
        }

        public override void WithdrawalCash(uint moneyToDraw)
        {
            if (OwnerBank.IsClientShady(AccountClient) && moneyToDraw > OwnerBank.Conditions.ShadyCustomersLimit)
            {
                throw new BanksException($"Error: shady client requested too many money to withdrawal " +
                    $"(requested {moneyToDraw}, but limit {OwnerBank.Conditions.ShadyCustomersLimit}");
            }

            var withdrawalCashCommand = new WithdrawalCashCommand(this, moneyToDraw);
            withdrawalCashCommand.Execute();
            if (Cash < _minAllowedCash)
            {
                withdrawalCashCommand.Undo();
                throw new BanksException($"Error: not enough money to withdrawal (requested {moneyToDraw}, but has {Cash})");
            }

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

            if (OwnerBank.IsClientShady(AccountClient) && moneyToTransfer > OwnerBank.Conditions.ShadyCustomersLimit)
            {
                throw new BanksException($"Error: shady client requested too many money to transfer " +
                    $"(requested {moneyToTransfer}, but limit {OwnerBank.Conditions.ShadyCustomersLimit}");
            }

            var transferCashCommand = new TransferCashCommand(this, targetBankAccount, moneyToTransfer);
            transferCashCommand.Execute();
            if (Cash < _minAllowedCash)
            {
                transferCashCommand.Undo();
                throw new BanksException($"Error: not enough money to transfer (requested {moneyToTransfer}, but has {Cash})");
            }

            CommandsHistory.Push(new Transaction(transferCashCommand, ActualDateTime));
        }

        public override void UndoCash()
        {
            if (CommandsHistory.Count == 0)
            {
                throw new BanksException("Error: no transactions to undo");
            }

            CommandsHistory.Peek().CashCommand.Undo();
            CommandsHistory.Pop();
        }

        internal override void UpdateCountedFee(DateTime dateTime)
        {
            if (Cash >= _minAllowedCash)
            {
                ActualDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            }
        }

        internal override void PayFee()
        {
        }
    }
}
