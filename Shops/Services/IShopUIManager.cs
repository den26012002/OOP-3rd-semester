using Shops.Entities;
using Shops.UI;

namespace Shops.Services
{
    public interface IShopUIManager
    {
        void Show();

        void SetActiveElement(IShopsUIElement uIElement);

        ShopsUISelectionMenu<T> CreateSelectionMenu<T>(string title);

        ShopsUIMultiSelectionMenu<T> CreateMultiSelectionMenu<T>(string title);
        ShopUIPersonMultiSelectionMenu<T> CreatePersonMultiSelectionMenu<T>(string title, Person person);
        ShopsUIShopSelectionMenu<T> CreateShopSelectionMenu<T>(string title, Shop shop);
        ShopsUIShopMultiSelectionMenu<T> CreateShopMultiSelectionMenu<T>(string title, Shop shop);
        ShopsUIInputField<T> CreateInputField<T>(string helpText);
        ShopsUIConfirmField CreateConfirmField(string helpText);
        ShopsUIConfirmField CreateConfirmField(Shop shop, uint price);
    }
}
