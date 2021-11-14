using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUICertainClientMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private Client _client;
        private BaseBanksUIState _returnState;

        public BanksUICertainClientMenuState(CentralBank centralBank, Client client, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _client = client;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            AnsiConsole.WriteLine("Информация о клиенте:");
            AnsiConsole.WriteLine($"Имя: {_client.Name}");
            AnsiConsole.WriteLine($"Фамилия: {_client.Surname}");
            AnsiConsole.Write($"Адрес:");
            if (_client.Address == null)
            {
                AnsiConsole.Write(" не указано");
            }
            else
            {
                AnsiConsole.Write($" улица {_client.Address.Street}, дом {_client.Address.HouseNumber}");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write($"Паспортные данные:");
            if (_client.PassportData == null)
            {
                AnsiConsole.Write(" не указано");
            }
            else
            {
                AnsiConsole.Write($" {_client.PassportData.Series} {_client.PassportData.Number}");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            AnsiConsole.Prompt(new SelectionPrompt<BanksUIMenuAction>()
                .Title("Выберите действие:")
                .AddChoices(
                new BanksUIMenuAction("1.Обновить информацию", OpenInformationUpdateMenu),
                new BanksUIMenuAction("2.Выбрать банк", OpenClientBankSelectionMenu),
                new BanksUIMenuAction("3.Вернуться", Return))).Invoke();
        }

        private void OpenInformationUpdateMenu()
        {
            var nextState = new BanksUIInformationUpdateMenuState(_centralBank, _client, this);
            Context.TransitionTo(nextState);
        }

        private void OpenClientBankSelectionMenu()
        {
            var nextState = new BanksUIClientBankSelectionMenuState(_centralBank, _client, this);
            Context.TransitionTo(nextState);
        }

        private void Return()
        {
            Context.TransitionTo(_returnState);
        }
    }
}
