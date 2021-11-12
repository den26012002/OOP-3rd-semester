namespace Banks.Entities
{
    public class TransferCashCommand : BaseCashCommand
    {
        private BaseBankAccount _targetBankAccount;
        private uint _moneyToTransfer;
        public TransferCashCommand(BaseBankAccount bankAccount, BaseBankAccount targetBankAccount, uint moneyToTransfer)
            : base(bankAccount)
        {
            _targetBankAccount = targetBankAccount;
            _moneyToTransfer = moneyToTransfer;
        }

        public override void Execute()
        {
            BankAccount.Cash -= (int)_moneyToTransfer;
            _targetBankAccount.Cash += (int)_moneyToTransfer;
        }

        public override void Undo()
        {
            _targetBankAccount.Cash -= (int)_moneyToTransfer;
            BankAccount.Cash += (int)_moneyToTransfer;
        }
    }
}
