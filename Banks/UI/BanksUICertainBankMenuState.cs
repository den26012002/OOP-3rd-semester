using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUICertainBankMenuState : BaseBanksUIState
    {
        private Bank _bank;
        private BaseBanksUIState _returnState;
        public BanksUICertainBankMenuState(Bank bank, BaseBanksUIState returnState)
        {
            _bank = bank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Настоящие условия:\n" + _bank.Conditions.ToString());
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действие:")
                .AddChoices(
                new BanksUIMenuAction("1.Обновить условия", OpenConditionsUpdateMenu),
                new BanksUIMenuAction("2.Вернуться", Return))).Invoke();
        }

        private void OpenConditionsUpdateMenu()
        {
            var nextState = new BanksUIConditionsUpdateMenuState(_bank, this);
            Context.TransitionTo(nextState);
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
