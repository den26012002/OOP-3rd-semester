using System;
using System.Collections.Generic;

namespace Banks.Entities
{
    public abstract class BaseBankAccount
    {
        internal protected BaseBankAccount(uint id, Bank ownerBank, Client client, int startCash)
        {
            Id = id;
            OwnerBank = ownerBank;
            AccountClient = client;
            Cash = startCash;
            CommandsHistory = new Stack<Transaction>();
            CountedFee = 0;
        }

        public uint Id { get; }
        public Bank OwnerBank { get; }
        public Client AccountClient { get; }
        public double Cash { get; internal set; }
        protected Stack<Transaction> CommandsHistory { get; set; }
        protected DateTime ActualDateTime { get; set; }
        protected double CountedFee { get; set; }

        public abstract void WithdrawalCash(uint moneyToDraw);

        public abstract void DepositCash(uint moneyToDeposit);

        public abstract void TransferCash(uint bankId, uint accountId, uint moneyToTransfer);

        public abstract void UndoCash();

        internal abstract void UpdateCountedFee(DateTime dateTime);

        internal abstract void PayFee();
    }
}
