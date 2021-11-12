using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIMainMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;

        public BanksUIMainMenuState(CentralBank centralBank)
        {
            _centralBank = centralBank;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите режим: ")
                .AddChoices(
                    new BanksUIMenuAction("1.Режим клиента", OpenClientMenu),
                    new BanksUIMenuAction("2.Режим банкира", OpenBankerMenu),
                    new BanksUIMenuAction("3.Режим хранителя времени", OpenTimeKeeperMenu))).Invoke();
        }

        private void OpenClientMenu()
        {
            var nextState = new BanksUIClientMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void OpenBankerMenu()
        {
            var nextState = new BanksUIBankerMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }

        private void OpenTimeKeeperMenu()
        {
            var nextState = new BanksUITimeKeeperMenuState(_centralBank, this);
            Context.TransitionTo(nextState);
        }
    }
}
