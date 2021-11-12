namespace Banks.Entities
{
    public class DepositCashCommand : BaseCashCommand
    {
        private uint _moneyToDeposit;
        public DepositCashCommand(BaseBankAccount bankAccount, uint moneyToDeposit)
            : base(bankAccount)
        {
            _moneyToDeposit = moneyToDeposit;
        }

        public override void Execute()
        {
            BankAccount.Cash += (int)_moneyToDeposit;
        }

        public override void Undo()
        {
            BankAccount.Cash -= (int)_moneyToDeposit;
        }
    }
}
