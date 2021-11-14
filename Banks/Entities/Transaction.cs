using System;

namespace Banks.Entities
{
    public class Transaction
    {
        public Transaction(BaseCashCommand cashCommand, DateTime transactionDate)
        {
            CashCommand = cashCommand;
            TransactionDate = transactionDate;
        }

        private Transaction()
        {
        }

        public int Id { get; private init; }
        public BaseCashCommand CashCommand { get; private init; }
        public DateTime TransactionDate { get; private init; }
    }
}
