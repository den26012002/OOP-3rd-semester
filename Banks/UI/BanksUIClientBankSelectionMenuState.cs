using System.Collections.Generic;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIClientBankSelectionMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private Client _client;
        private BaseBanksUIState _returnState;

        public BanksUIClientBankSelectionMenuState(CentralBank centralBank, Client client, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _client = client;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            BaseBanksUIState nextState = null;
            var bankActions = new List<BanksUIMenuAction>();
            foreach (Bank bank in _centralBank.Banks)
            {
                bankActions.Add(new BanksUIMenuAction(bank.Name, () => nextState = new BanksUIClientBankAccountsMenuState(bank, _client, this)));
            }

            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите банк:")
                .AddChoices(bankActions.ToArray())
                .AddChoices(new BanksUIMenuAction("Вернуться", () => nextState = _returnState))).Invoke();
            Context.TransitionTo(nextState);
        }
    }
}
