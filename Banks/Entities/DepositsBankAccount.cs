using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositsBankAccount : BaseBankAccount
    {
        private readonly int _minAllowedCash = 0;
        private DateTime _expirationTime;
        private bool _isExpired;
        private int _startCash;

        internal DepositsBankAccount(uint id, Bank bankOwner, Client accountClient, int startCash, DateTime expirationTime)
            : base(id, bankOwner, accountClient, startCash)
        {
            if (startCash < 0)
            {
                throw new BanksException("Error: money on deposits account can't be less than 0");
            }

            _startCash = startCash;
            _expirationTime = expirationTime;
            _isExpired = false;
        }

        public override void WithdrawalCash(uint moneyToDraw)
        {
            if (OwnerBank.IsClientShady(AccountClient) && moneyToDraw > OwnerBank.Conditions.ShadyCustomersLimit)
            {
                throw new BanksException($"Error: shady client requested too many money to withdrawal " +
                    $"(requested {moneyToDraw}, but limit {OwnerBank.Conditions.ShadyCustomersLimit}");
            }

            if (!_isExpired)
            {
                throw new BanksException($"Error: can't withdrawal money till expiration time ({_expirationTime})");
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

            if (!_isExpired)
            {
                throw new BanksException($"Error: can't transfer money till expiration time ({_expirationTime})");
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
            if (dateTime > _expirationTime)
            {
                _isExpired = true;
                dateTime = _expirationTime;
            }

            if (Cash >= _minAllowedCash)
            {
                TimeSpan deltaTime = dateTime - ActualDateTime;
                CountedFee += (int)((double)deltaTime.Days * Cash * FindPercents(_startCash) / 100.0);

                ActualDateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
            }
        }

        internal override void PayFee()
        {
            Cash += CountedFee;
            CountedFee = 0;
        }

        private double FindPercents(int cash)
        {
            double resultPercents = 0;
            foreach (DepositFee fee in OwnerBank.Conditions.DepositFees)
            {
                if (cash >= fee.Limit)
                {
                    resultPercents = fee.Percents;
                }
            }

            return resultPercents;
        }
    }
}
