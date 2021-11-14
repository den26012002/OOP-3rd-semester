using System.Collections.Generic;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIClientsListMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;

        public BanksUIClientsListMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            var clientActions = new List<BanksUIMenuAction>();
            BaseBanksUIState nextState = null;
            foreach (Client client in _centralBank.CentralClientsRegistrator.Clients)
            {
                clientActions.Add(new BanksUIMenuAction(
                    $"{client.Name} {client.Surname}",
                    () => nextState = new BanksUICertainClientMenuState(_centralBank, client, this)));
            }

            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите клиента")
                .AddChoices(clientActions.ToArray())
                .AddChoices(new BanksUIMenuAction("Вернуться", () => nextState = _returnState))).Invoke();
            Context.TransitionTo(nextState);
        }
    }
}
