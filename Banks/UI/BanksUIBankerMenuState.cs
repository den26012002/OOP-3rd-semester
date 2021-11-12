using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIBankerMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;

        public BanksUIBankerMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действия:")
                .AddChoices(
                    new BanksUIMenuAction("1.Показать список банков", OpenBanksListMenu),
                    new BanksUIMenuAction("2.Зарегистрировать новый банк", OpenBanksRegistrationMenu),
                    new BanksUIMenuAction("3.Вернуться в главное меню", Return))).Invoke();
        }

        private void OpenBanksListMenu()
        {
            var nextState = new BanksUIBanksListMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void OpenBanksRegistrationMenu()
        {
            var nextState = new BanksUIBankRegistrationMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
