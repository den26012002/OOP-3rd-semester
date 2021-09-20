using Shops.Entities;
using Shops.Services;

namespace Shops.UI
{
    public class ShopUIPersonMultiSelectionMenu<T> : ShopsUIMultiSelectionMenu<T>
    {
        public ShopUIPersonMultiSelectionMenu(IShopUIManager uIManager, string title, Person person)
            : base(uIManager, title)
        {
            Person = person;
        }

        public Person Person { get; }

        public override void Show()
        {
            string title = Title;
            ChangeTitle(title + $"(у вас [yellow]{Person.Money} золотых[/])");
            base.Show();
            ChangeTitle(title);
        }
    }
}
