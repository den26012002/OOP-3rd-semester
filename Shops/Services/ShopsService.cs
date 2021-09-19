/*using System;
using System.Collections.Generic;
using Shops.Entities;
using Shops.Tools;
using Shops.UI;*/

namespace Shops.Services
{
    public class ShopsService
    {
        /*private IShopManager _manager;
        private IShopUIManager _uIManager;

        private Person _customer;
        private Dictionary<Product, uint> _shoppingList;
        private Dictionary<Product, ProductInfo> _deliveryList;

        private ShopsUISelectionMenu<string> mainMenu;
        private ShopsUISelectionMenu<string> customerMenu;
        private ShopsUISelectionMenu<string> sellerMenu;
        private ShopsUISelectionMenu<string> managerMenu;
        public ShopsService(IShopManager manager, IShopUIManager uIManager)
        {
            _manager = manager;
            _uIManager = uIManager;
            _customer = new Person("User", 100);
            _shoppingList = new Dictionary<Product, uint>();
            _deliveryList = new Dictionary<Product, ProductInfo>();
            MakeUI();
        }

        public void Run()
        {
            _uIManager.Show();
        }

        private void MakeUI()
        {
            mainMenu = _uIManager.CreateSelectionMenu<string>("Выберите режим: ");
            _uIManager.SetActiveElement(mainMenu);
            customerMenu = _uIManager.CreateSelectionMenu<string>("Выберите действие: ");
            sellerMenu = _uIManager.CreateSelectionMenu<string>("Выберите магазин: ");
            managerMenu = _uIManager.CreateSelectionMenu<string>("Выберите действие: ");
            mainMenu.AddChildElement(customerMenu);
            mainMenu.AddChildElement(sellerMenu);
            mainMenu.AddChildElement(managerMenu);
            MakeCustomerMenu(customerMenu);
            MakeSellerMenu(sellerMenu);
            MakeManagerMenu(managerMenu);
            mainMenu.AddSelectionAction(new MenuAction<string>("1.Режим покупателя", () => { _uIManager.SetActiveElement(customerMenu); }));
            mainMenu.AddSelectionAction(new MenuAction<string>("2.Режим продавца", () =>
            {
                UpdateSellerMenu(sellerMenu);
                _uIManager.SetActiveElement(sellerMenu);
            }));
            mainMenu.AddSelectionAction(new MenuAction<string>("3.Режим менеджера", () => { _uIManager.SetActiveElement(managerMenu); }));
        }

        private void MakeCustomerMenu(ShopsUISelectionMenu<string> customerMenu)
        {
            ShopUIPersonMultiSelectionMenu<string> customerBuyingMenu = _uIManager.CreatePersonMultiSelectionMenu<string>("Выберите товары:", _customer);
            customerMenu.AddChildElement(customerBuyingMenu);
            MakeCustomerBuyingMenu(customerBuyingMenu);

            customerMenu.AddSelectionAction(new MenuAction<string>("1.Заработать денег", () => { _customer.EarnMoney((uint)new Random().Next() % 10); }));
            customerMenu.AddSelectionAction(new MenuAction<string>("2.Купить товары", () =>
            {
                UpdateCustomerBuyingMenu(customerBuyingMenu);
                _uIManager.SetActiveElement(customerBuyingMenu);
            }));
            customerMenu.AddSelectionAction(new MenuAction<string>("3.Вернуться в главное меню", () => { _uIManager.SetActiveElement(customerMenu.ParentElement); }));
        }

        private void MakeCustomerBuyingMenu(ShopUIPersonMultiSelectionMenu<string> customerBuyingMenu)
        {
            ShopsUISelectionMenu<string> customerAcceptBuyingMenu = _uIManager.CreateSelectionMenu<string>("Как купить?");
            ShopsUISelectionMenu<string> errorMenu = _uIManager.CreateSelectionMenu<string>("Подходящий магазин не найден");
            errorMenu.AddSelectionAction(new MenuAction<string>("Вернуться", () => { _uIManager.SetActiveElement(errorMenu.ParentElement); }));
            customerAcceptBuyingMenu.AddChildElement(errorMenu);

            ShopsUISelectionMenu<string> customerShopSelectionMenu = _uIManager.CreateSelectionMenu<string>("Выберите магазин:");
            MakeCustomerShopSelectionMenu(customerShopSelectionMenu);

            customerAcceptBuyingMenu.AddSelectionAction(new MenuAction<string>("1.Купить дёшево", () =>
            {
                Shop cheapestShop = _manager.FindCheapestShop(_shoppingList);
                if (cheapestShop == null)
                {
                    _uIManager.SetActiveElement(errorMenu);
                    return;
                }

                try
                {
                    ShopsUIConfirmField confirmField = _uIManager.CreateConfirmField(cheapestShop, cheapestShop.GetPrice(_shoppingList));
                    confirmField.Show();
                    if (confirmField.GetResult())
                    {
                        cheapestShop.Buy(_customer, _shoppingList);
                        _uIManager.SetActiveElement(customerMenu);
                    }
                }
                catch (ShopsException exception)
                {
                    var errorMenu = (ShopsUISelectionMenu<string>)customerShopSelectionMenu.ChildElements[0];
                    errorMenu.ChangeTitle(exception.Message);
                    _uIManager.SetActiveElement(errorMenu);
                }
            }));
            customerAcceptBuyingMenu.AddSelectionAction(new MenuAction<string>("2.Купить в конкретном магазине", () =>
            {
                UpdateCustomerShopSelectionMenu(customerShopSelectionMenu);
                _uIManager.SetActiveElement(customerShopSelectionMenu);
            }));
            customerAcceptBuyingMenu.AddSelectionAction(new MenuAction<string>("3.Отменить покупку", () => { _uIManager.SetActiveElement(customerAcceptBuyingMenu.ParentElement.ParentElement); }));
            customerAcceptBuyingMenu.AddChildElement(customerShopSelectionMenu);

            customerBuyingMenu.AddChildElement(customerAcceptBuyingMenu);
        }

        private void UpdateCustomerBuyingMenu(ShopUIPersonMultiSelectionMenu<string> customerBuyingMenu)
        {
            customerBuyingMenu.ClearActions();

            customerBuyingMenu.AddPreSelectionAction(new MenuAction<string>("Clearing the shoppingList", () => { _shoppingList.Clear(); }));

            foreach (Product product in _manager.Products)
            {
                customerBuyingMenu.AddSelectionAction(new MenuAction<string>(product.Name, () =>
                {
                    _shoppingList.Add(product, 0);
                    ShopsUIInputField<uint> productBuyingNumberField = _uIManager.CreateInputField<uint>($"Введите количество товара \"{product.Name}\":");
                    productBuyingNumberField.Show();
                    _shoppingList[product] = productBuyingNumberField.GetResult();
                }));
            }

            customerBuyingMenu.AddPostSelectionAction(new MenuAction<string>("Show next menu", () => { _uIManager.SetActiveElement(customerBuyingMenu.ChildElements[0]); }));
        }

        private void MakeCustomerShopSelectionMenu(ShopsUISelectionMenu<string> customerShopSelectionMenu)
        {
            ShopsUISelectionMenu<string> errorMenu = _uIManager.CreateSelectionMenu<string>(string.Empty);
            errorMenu.AddSelectionAction(new MenuAction<string>("Вернуться", () =>
            {
                UpdateCustomerShopSelectionMenu(customerShopSelectionMenu);
                _uIManager.SetActiveElement(customerShopSelectionMenu.ParentElement);
            }));
            customerShopSelectionMenu.AddChildElement(errorMenu);
        }

        private void UpdateCustomerShopSelectionMenu(ShopsUISelectionMenu<string> customerShopSelectionMenu)
        {
            customerShopSelectionMenu.ClearActions();
            foreach (Shop shop in _manager.Shops)
            {
                customerShopSelectionMenu.AddSelectionAction(new MenuAction<string>(shop.Name, () =>
                {
                    try
                    {
                        ShopsUIConfirmField confirmField = _uIManager.CreateConfirmField(shop, shop.GetPrice(_shoppingList));
                        confirmField.Show();
                        if (confirmField.GetResult())
                        {
                            shop.Buy(_customer, _shoppingList);
                            _uIManager.SetActiveElement(customerMenu);
                        }
                    }
                    catch (ShopsException exception)
                    {
                        var errorMenu = (ShopsUISelectionMenu<string>)customerShopSelectionMenu.ChildElements[0];
                        errorMenu.ChangeTitle(exception.Message);
                        _uIManager.SetActiveElement(errorMenu);
                    }
                }));
            }

            customerShopSelectionMenu.AddSelectionAction(new MenuAction<string>("Вернуться к выбору способа покупки", () => { _uIManager.SetActiveElement(customerShopSelectionMenu.ParentElement); }));
        }

        private void MakeSellerMenu(ShopsUISelectionMenu<string> sellerMenu)
        {
            ShopsUIShopSelectionMenu<string> shopMenu = _uIManager.CreateShopSelectionMenu<string>("Выберите действие", null);
            sellerMenu.AddChildElement(shopMenu);

            ShopsUIShopMultiSelectionMenu<string> deliveryMenu = _uIManager.CreateShopMultiSelectionMenu<string>("Выберите товары: ", null);
            shopMenu.AddChildElement(deliveryMenu);
        }

        private void UpdateSellerMenu(ShopsUISelectionMenu<string> sellerMenu)
        {
            sellerMenu.ClearActions();
            foreach (Shop shop in _manager.Shops)
            {
                sellerMenu.AddSelectionAction(new MenuAction<string>(shop.Name, () =>
                {
                    ((ShopsUIShopSelectionMenu<string>)sellerMenu.ChildElements[0]).ChangeShop(shop);
                    UpdateShopMenu((ShopsUIShopSelectionMenu<string>)sellerMenu.ChildElements[0]);
                    _uIManager.SetActiveElement(sellerMenu.ChildElements[0]);
                }));
            }

            sellerMenu.AddSelectionAction(new MenuAction<string>("Вернуться в главное меню", () => { _uIManager.SetActiveElement(sellerMenu.ParentElement); }));
        }

        private void UpdateShopMenu(ShopsUIShopSelectionMenu<string> shopMenu)
        {
            shopMenu.ClearActions();
            shopMenu.AddSelectionAction(new MenuAction<string>("1.Заказать доставку", () =>
            {
                ((ShopsUIShopMultiSelectionMenu<string>)shopMenu.ChildElements[0]).ChangeShop(shopMenu.Shop);
                UpdateDeliveryMenu((ShopsUIShopMultiSelectionMenu<string>)shopMenu.ChildElements[0]);
                _uIManager.SetActiveElement(shopMenu.ChildElements[0]);
            }));
            shopMenu.AddSelectionAction(new MenuAction<string>("2.Вернуться", () => { _uIManager.SetActiveElement(shopMenu.ParentElement); }));
        }

        private void UpdateDeliveryMenu(ShopsUIShopMultiSelectionMenu<string> deliveryMenu)
        {
            deliveryMenu.ClearActions();
            deliveryMenu.AddPreSelectionAction(new MenuAction<string>("Clear delivery list", () => { _deliveryList.Clear(); }));
            foreach (Product product in _manager.Products)
            {
                deliveryMenu.AddSelectionAction(new MenuAction<string>(product.Name, () =>
                {
                    _deliveryList.Add(product, new ProductInfo(0, 0));
                    ShopsUIInputField<uint> productDeliveryNumberField = _uIManager.CreateInputField<uint>($"Введите количество товара {product.Name}:");
                    ShopsUIInputField<uint> productDeliveryPriceField = _uIManager.CreateInputField<uint>($"Введите цену на товар {product.Name}:");
                    productDeliveryNumberField.Show();
                    productDeliveryPriceField.Show();
                    _deliveryList[product].Count = productDeliveryNumberField.GetResult();
                    _deliveryList[product].Price = productDeliveryPriceField.GetResult();
                }));
            }

            deliveryMenu.AddPostSelectionAction(new MenuAction<string>("Delivery products", () => { deliveryMenu.Shop.AddProducts(_deliveryList); }));
            deliveryMenu.AddPostSelectionAction(new MenuAction<string>("Return", () => { _uIManager.SetActiveElement(deliveryMenu.ParentElement); }));
        }

        private void MakeManagerMenu(ShopsUISelectionMenu<string> managerMenu)
        {
            ShopsUIInputField<string> productRegistrationField = _uIManager.CreateInputField<string>("Введите название продукта:");
            ShopsUIInputField<string> shopRegistrationNameField = _uIManager.CreateInputField<string>("Введите название магазина:");
            ShopsUIInputField<string> shopRegistrationStreetField = _uIManager.CreateInputField<string>("Введите название улицы:");
            ShopsUIInputField<uint> shopRegistrationHouseNumberField = _uIManager.CreateInputField<uint>("Введите номер дома:");

            managerMenu.AddSelectionAction(new MenuAction<string>("1.Зарегистрировать продукт", () =>
            {
                productRegistrationField.Show();
                _manager.RegisterProduct(productRegistrationField.GetResult());
            }));
            managerMenu.AddSelectionAction(new MenuAction<string>("2.Зарегистрировать магазин", () =>
            {
                shopRegistrationNameField.Show();
                shopRegistrationStreetField.Show();
                shopRegistrationHouseNumberField.Show();
                _manager.Create(
                    shopRegistrationNameField.GetResult(),
                    new Address(shopRegistrationStreetField.GetResult(), shopRegistrationHouseNumberField.GetResult()));
            }));
            managerMenu.AddSelectionAction(new MenuAction<string>("3.Вернуться в главное меню", () => { _uIManager.SetActiveElement(managerMenu.ParentElement); }));
        }*/
    }
}
