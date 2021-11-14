using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUITimeKeeperMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;
        public BanksUITimeKeeperMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"Сегодня {_centralBank.GetDateNow()}");
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действие: ")
                .AddChoices(
                    new BanksUIMenuAction("1.Прокрутить время", OpenTimeScrollingMenu),
                    new BanksUIMenuAction("2.Вернуться в главное меню", Return))).Invoke();
        }

        private void OpenTimeScrollingMenu()
        {
            var nextState = new BanksUITimeScrollingMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
