using Shops.Entities;
using Shops.Services;

namespace Shops.UI
{
    public class ShopsUIShopMultiSelectionMenu<T> : ShopsUIMultiSelectionMenu<T>
    {
        public ShopsUIShopMultiSelectionMenu(IShopUIManager uIManager, string title, Shop shop)
            : base(uIManager, title)
        {
            Shop = shop;
        }

        public Shop Shop { get; private set; }

        public void ChangeShop(Shop newShop)
        {
            Shop = newShop;
        }
    }
}
