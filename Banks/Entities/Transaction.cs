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

        public BaseCashCommand CashCommand { get; }
        public DateTime TransactionDate { get; }
    }
}
