using System;
using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIBankAccountRegistrationMenuState : BaseBanksUIState
    {
        private Bank _bank;
        private Client _client;
        private BaseBanksUIState _returnState;

        public BanksUIBankAccountRegistrationMenuState(Bank bank, Client client, BaseBanksUIState returnState)
        {
            _bank = bank;
            _client = client;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            string accountTypeString = AnsiConsole.Prompt(new TextPrompt<string>("Введите тип счёта:")
                .AddChoice("Дебетовый")
                .AddChoice("Депозитный")
                .AddChoice("Кредитный"));
            AccountType accountType = default;
            DateTime expirationDateTime = default;
            switch (accountTypeString)
            {
                case "Дебетовый":
                    accountType = AccountType.Debit;
                    break;
                case "Депозитный":
                    accountType = AccountType.Deposit;
                    int expirationYear = AnsiConsole.Prompt(new TextPrompt<int>("Введите год истечения:"));
                    int expirationMonth = AnsiConsole.Prompt(new TextPrompt<int>("Введите месяц истечения:")
                        .Validate((int numb) => { return numb > 0 && numb <= 12; }));
                    expirationDateTime = new DateTime(expirationYear, expirationMonth, 1);
                    break;
                case "Кредитный":
                    accountType = AccountType.Credit;
                    break;
            }

            int startCash = AnsiConsole.Prompt(new TextPrompt<int>("Введите начальную сумму:"));
            _bank.AddBankAccount(_client, accountType, startCash, expirationDateTime);
            Context.TransitionTo(_returnState);
        }
    }
}
