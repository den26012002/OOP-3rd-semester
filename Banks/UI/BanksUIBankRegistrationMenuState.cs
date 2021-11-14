using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIBankRegistrationMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;

        public BanksUIBankRegistrationMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            string bankName = AnsiConsole.Prompt(new TextPrompt<string>("Введите название банка:"));
            Bank newBank = _centralBank.AddBank(bankName);
            var nextState = new BanksUIConditionsUpdateMenuState(newBank, _returnState);
            Context.TransitionTo(nextState);
        }
    }
}
