namespace Banks.Entities
{
    public abstract class BaseCashCommand
    {
        protected BaseCashCommand(BaseBankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }

        protected BaseBankAccount BankAccount { get; }

        public abstract void Execute();
        public abstract void Undo();
    }
}
