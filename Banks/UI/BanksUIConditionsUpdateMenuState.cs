using System.Collections.Generic;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIConditionsUpdateMenuState : BaseBanksUIState
    {
        private Bank _bank;
        private BaseBanksUIState _returnState;

        public BanksUIConditionsUpdateMenuState(Bank bank, BaseBanksUIState returnState)
        {
            _bank = bank;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            int shadyClientsLimit = AnsiConsole.Prompt(new TextPrompt<int>("Введите лимит для подозрительных клиентов:"));
            uint creditFee = AnsiConsole.Prompt(new TextPrompt<uint>("Введите комиссию за пользование кредитным счётом:"));
            int depositFeesBordersNumber = AnsiConsole.Prompt(new TextPrompt<int>("Введите количество границ для процентов на депозитном счёте:"));
            var depositFees = new List<DepositFee>();
            for (int i = 0; i < depositFeesBordersNumber; ++i)
            {
                int limit = AnsiConsole.Prompt(new TextPrompt<int>("Введите минимальную сумму, с которой будет начисляться процент:"));
                double percents = AnsiConsole.Prompt(new TextPrompt<double>("Введите величину процента:"));
                depositFees.Add(new DepositFee(limit, percents));
            }

            var newConditions = new BankConditions(creditFee, depositFees, shadyClientsLimit);
            _bank.UpdateConditions(newConditions);
            Context.TransitionTo(_returnState);
        }
    }
}
