using Shops.Entities;
using Shops.UI;
using Spectre.Console;

namespace Shops.Services
{
    public class ShopUIManager : IShopUIManager
    {
        private IShopsUIElement _activeElement;
        public void Show()
        {
            while (true)
            {
                AnsiConsole.Console.Clear();
                _activeElement.Show();
            }
        }

        public void SetActiveElement(IShopsUIElement uIElement)
        {
            _activeElement = uIElement;
        }

        public ShopsUISelectionMenu<T> CreateSelectionMenu<T>(string title)
        {
            return new ShopsUISelectionMenu<T>(this, title);
        }

        public ShopsUIMultiSelectionMenu<T> CreateMultiSelectionMenu<T>(string title)
        {
            return new ShopsUIMultiSelectionMenu<T>(this, title);
        }

        public ShopUIPersonMultiSelectionMenu<T> CreatePersonMultiSelectionMenu<T>(string title, Person person)
        {
            return new ShopUIPersonMultiSelectionMenu<T>(this, title, person);
        }

        public ShopsUIShopSelectionMenu<T> CreateShopSelectionMenu<T>(string title, Shop shop)
        {
            return new ShopsUIShopSelectionMenu<T>(this, title, shop);
        }

        public ShopsUIShopMultiSelectionMenu<T> CreateShopMultiSelectionMenu<T>(string title, Shop shop)
        {
            return new ShopsUIShopMultiSelectionMenu<T>(this, title, shop);
        }

        public ShopsUIInputField<T> CreateInputField<T>(string helpText)
        {
            return new ShopsUIInputField<T>(this, helpText);
        }

        public ShopsUIConfirmField CreateConfirmField(string helpText)
        {
            return new ShopsUIConfirmField(this, helpText);
        }

        public ShopsUIConfirmField CreateConfirmField(Shop shop, uint price)
        {
            return new ShopsUIConfirmField(this, $"Вы действительно хотите купить в магазине {shop.Name} за [yellow]{price} золотых[/]");
        }
    }
}
