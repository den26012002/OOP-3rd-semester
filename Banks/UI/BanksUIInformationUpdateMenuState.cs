using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIInformationUpdateMenuState : BaseBanksUIState
    {
        private CentralBank _centralBank;
        private Client _client;
        private BaseBanksUIState _returnState;

        public BanksUIInformationUpdateMenuState(CentralBank centralBank, Client client, BaseBanksUIState returnState)
        {
            _centralBank = centralBank;
            _client = client;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите обновить имя?")))
            {
                _client.Name = AnsiConsole.Prompt(new TextPrompt<string>("Введите имя:"));
            }

            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите обновить фамилию?")))
            {
                _client.Surname = AnsiConsole.Prompt(new TextPrompt<string>("Введите фамилию:"));
            }

            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите обновить адрес?")))
            {
                string streetName = AnsiConsole.Prompt(new TextPrompt<string>("Введите название улицы:"));
                uint houseNumber = AnsiConsole.Prompt(new TextPrompt<uint>("Введите номер дома:"));
                _client.Address = new Address(streetName, houseNumber);
            }

            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите обновить паспортные данные?")))
            {
                uint passportSeries = AnsiConsole.Prompt(new TextPrompt<uint>("Введите серию паспорта:"));
                uint passportNumber = AnsiConsole.Prompt(new TextPrompt<uint>("Введите номер паспорта:"));
                _client.PassportData = new PassportData(passportSeries, passportNumber);
            }

            Context.TransitionTo(_returnState);
        }
    }
}
