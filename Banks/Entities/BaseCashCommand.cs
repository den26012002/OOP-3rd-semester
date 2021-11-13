namespace Banks.Entities
{
    public abstract class BaseCashCommand
    {
        protected BaseCashCommand(BaseBankAccount bankAccount)
        {
            BankAccount = bankAccount;
        }

        protected BaseCashCommand()
        {
        }

        public int Id { get; private init; }
        protected BaseBankAccount BankAccount { get; private init; }

        public abstract void Execute();
        public abstract void Undo();
    }
}
