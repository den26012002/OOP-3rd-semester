using Banks.Entities;
using Banks.Tools;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIBankAccountMenuState : BaseBanksUIState
    {
        private BaseBankAccount _bankAccount;
        private BaseBanksUIState _returnState;

        public BanksUIBankAccountMenuState(BaseBankAccount bankAccount, BaseBanksUIState returnState)
        {
            _bankAccount = bankAccount;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine($"Счёт №{_bankAccount.Id}");
            AnsiConsole.WriteLine($"Сумма на сегодня: {_bankAccount.Cash}");
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действие:")
                .AddChoices(
                    new BanksUIMenuAction("1.Снять наличные", WithdrawalCash),
                    new BanksUIMenuAction("2.Пополнить счёт", DepositCash),
                    new BanksUIMenuAction("3.Перевести деньги на другой счёт", TransferCash),
                    new BanksUIMenuAction("4.Отменить предыдущую операцию", UndoCash),
                    new BanksUIMenuAction("5.Вернуться", Return))).Invoke();
        }

        private void WithdrawalCash()
        {
            uint moneyToDraw = AnsiConsole.Prompt(new TextPrompt<uint>("Введите сумму, которую хотите снять:"));
            try
            {
                _bankAccount.WithdrawalCash(moneyToDraw);
            }
            catch (BanksException exception)
            {
                AnsiConsole.WriteLine(exception.Message);
                AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title(string.Empty)
                    .AddChoices("Вернуться к выбору"));
            }
        }

        private void DepositCash()
        {
            uint moneyToDeposit = AnsiConsole.Prompt(new TextPrompt<uint>("Введите сумму, которую хотите положить:"));
            try
            {
                _bankAccount.DepositCash(moneyToDeposit);
            }
            catch (BanksException exception)
            {
                AnsiConsole.WriteLine(exception.Message);
                AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title(string.Empty)
                    .AddChoices("Вернуться к выбору"));
            }
        }

        private void TransferCash()
        {
            try
            {
                uint bankId = AnsiConsole.Prompt(new TextPrompt<uint>("Введите id банка, в который хотите совершить перевод:"));
                uint accountId = AnsiConsole.Prompt(new TextPrompt<uint>("Введите id клиента, которому хотите перевести деньги:"));
                uint moneyToTransfer = AnsiConsole.Prompt(new TextPrompt<uint>("Введите сумму:"));
                _bankAccount.TransferCash(bankId, accountId, moneyToTransfer);
            }
            catch (BanksException exception)
            {
                AnsiConsole.WriteLine(exception.Message);
                AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title(string.Empty)
                    .AddChoices("Вернуться к выбору"));
            }
        }

        private void UndoCash()
        {
            try
            {
                _bankAccount.UndoCash();
            }
            catch (BanksException exception)
            {
                AnsiConsole.WriteLine(exception.Message);
                AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title(string.Empty)
                    .AddChoices("Вернуться к выбору"));
            }
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
