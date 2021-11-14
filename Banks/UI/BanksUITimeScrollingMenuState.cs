using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUITimeScrollingMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private BaseBanksUIState _returnState;
        public BanksUITimeScrollingMenuState(CentralBank centralBank, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            int numberOfDays = AnsiConsole.Prompt(new TextPrompt<int>("Введите количество дней, на которое нужно промотать время:"));
            _centralBank.ScrollTime(numberOfDays);
            Context.TransitionTo(_returnState);
        }
    }
}
