using System.Collections.Generic;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIBanksListMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;

        public BanksUIBanksListMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            var bankActions = new List<BanksUIMenuAction>();
            BaseBanksUIState nextState = null;
            foreach (Bank bank in _centralBank.Banks)
            {
                bankActions.Add(new BanksUIMenuAction(bank.Name, () => nextState = new BanksUICertainBankMenuState(bank, this)));
            }

            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите банк:")
                .AddChoices(bankActions.ToArray())
                .AddChoices(new BanksUIMenuAction("Вернуться", () => nextState = _returnState))).Invoke();

            Context.TransitionTo(nextState);
        }
    }
}
