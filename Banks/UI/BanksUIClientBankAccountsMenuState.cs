using System.Collections.Generic;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIClientBankAccountsMenuState : BaseBanksUIState
    {
        private Bank _bank;
        private Client _client;
        private BaseBanksUIState _returnState;

        public BanksUIClientBankAccountsMenuState(Bank bank, Client client, BaseBanksUIState returnState)
        {
            _bank = bank;
            _client = client;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            BaseBanksUIState nextState = null;
            var bankAccountActions = new List<BanksUIMenuAction>();
            foreach (BaseBankAccount bankAccount in _bank.BankAccounts)
            {
                if (bankAccount.AccountClient == _client)
                {
                    bankAccountActions.Add(new BanksUIMenuAction(
                        bankAccount.Id.ToString(),
                        () => nextState = new BanksUIBankAccountMenuState(bankAccount, this)));
                }
            }

            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите счёт:")
                .AddChoices(bankAccountActions.ToArray())
                .AddChoices(
                    new BanksUIMenuAction(
                        "Зарегистрировать новый счёт",
                        () => nextState = new BanksUIBankAccountRegistrationMenuState(_bank, _client, this)),
                    new BanksUIMenuAction("Вернуться", () => nextState = _returnState))).Invoke();
            Context.TransitionTo(nextState);
        }
    }
}
