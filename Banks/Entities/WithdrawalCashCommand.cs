namespace Banks.Entities
{
    public class WithdrawalCashCommand : BaseCashCommand
    {
        private uint _moneyToDraw;
        public WithdrawalCashCommand(BaseBankAccount bankAccount, uint moneyToDraw)
            : base(bankAccount)
        {
            _moneyToDraw = moneyToDraw;
        }

        public override void Execute()
        {
            BankAccount.Cash -= (int)_moneyToDraw;
        }

        public override void Undo()
        {
            BankAccount.Cash += (int)_moneyToDraw;
        }
    }
}
