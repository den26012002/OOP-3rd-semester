using Banks.Entities;
using Spectre.Console;

namespace Banks.UI
{
    public class BanksUIClientRegistrationMenuState : BaseBanksUIState
    {
        private ClientsRegistrator _clientsRegistrator;
        private BaseBanksUIState _returnState;

        public BanksUIClientRegistrationMenuState(ClientsRegistrator clientsRegistrator, BaseBanksUIState returnState)
        {
            _clientsRegistrator = clientsRegistrator;
            _returnState = returnState;
        }

        public override void Show()
        {
            AnsiConsole.Clear();
            string clientName = AnsiConsole.Prompt(new TextPrompt<string>("Введите имя:"));
            string clientSurname = AnsiConsole.Prompt(new TextPrompt<string>("Введите фамилию:"));
            Address clientAddress = null;
            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите ввести адрес?")))
            {
                string streetName = AnsiConsole.Prompt(new TextPrompt<string>("Введите название улицы:"));
                uint houseNumber = AnsiConsole.Prompt(new TextPrompt<uint>("Введите номер дома:"));
                clientAddress = new Address(streetName, houseNumber);
            }

            PassportData clientPassportData = null;
            if (AnsiConsole.Prompt(new ConfirmationPrompt("Хотите ввести паспортные данные?")))
            {
                uint passportSeries = AnsiConsole.Prompt(new TextPrompt<uint>("Введите серию паспорта:"));
                uint passportNumber = AnsiConsole.Prompt(new TextPrompt<uint>("Введите номер паспорта:"));
                clientPassportData = new PassportData(passportSeries, passportNumber);
            }

            _clientsRegistrator.RegisterClient(clientName, clientSurname, clientAddress, clientPassportData);
            Context.TransitionTo(_returnState);
        }
    }
}
