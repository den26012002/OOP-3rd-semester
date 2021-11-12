using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIClientMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;

        public BanksUIClientMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действие:")
                .AddChoices(
                new BanksUIMenuAction("1.Показать список клиентов", OpenClientsListMenu),
                new BanksUIMenuAction("2.Зарегистрировать нового клиента", OpenClientRegistrationMenu),
                new BanksUIMenuAction("3.Вернуться", Return))).Invoke();
        }

        private void OpenClientsListMenu()
        {
            var nextState = new BanksUIClientsListMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void OpenClientRegistrationMenu()
        {
            var nextState = new BanksUIClientRegistrationMenuState(_centralBank.CentralClientsRegistrator, this);
            Context.TransitionTo(nextState);
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
