using Shops.Services;
using Shops.Entities;
using Shops.Tools;
using NUnit.Framework;

namespace Shops.Tests
{
    public class Tests
    {
        [Test]
        [TestCase(10u)]
        public void AddProductsToShop_ShopContainsProducts(uint productCount)
        {
            var shopManager = new ShopManager();
            var shop = shopManager.Create("shop name", new Address("street", 10));
            var product = shopManager.RegisterProduct("product name");

            var deliveryList = shopManager.CreateDeliveryList();
            deliveryList.Add(product, new ProductInfo(productCount, 100));
            shop.AddProducts(deliveryList);

            Assert.AreEqual(shop.HasProduct(product, productCount), true);
        }

        [Test]
        [TestCase(10u, 9u)]
        public void ChangePriceAtShop_PriceChanged(uint startPrice, uint newPrice)
        {
            var shopManager = new ShopManager();
            var shop = shopManager.Create("shop name", new Address("street", 10));
            var product = shopManager.RegisterProduct("product name");

            var deliveryList = shopManager.CreateDeliveryList();
            deliveryList.Add(product, new ProductInfo(10, startPrice));
            shop.AddProducts(deliveryList);
            Assert.AreEqual(shop.GetProductInfo(product).Price, startPrice);

            shop.UpdatePrice(product, newPrice);
            Assert.AreEqual(shop.GetProductInfo(product).Price, newPrice);
            
        }

        [Test]
        public void FindCheapestShopAndShopsHasNoOrNotEnoughProducts_ReturnNull()
        {
            var shopManager = new ShopManager();
            var pyaterochka = shopManager.Create("Pyaterochka", new Address("street", 10));
            var dixi = shopManager.Create("Dixi", new Address("another street", 20));
            shopManager.Create("Okey", new Address("one more street", 15));

            var food = shopManager.RegisterProduct("food");
            var clothes = shopManager.RegisterProduct("clothes");
            var education = shopManager.RegisterProduct("education");

            var deliveryList = shopManager.CreateDeliveryList();
            deliveryList.Add(food, new ProductInfo(15, 20));
            deliveryList.Add(clothes, new ProductInfo(10, 10));
            pyaterochka.AddProducts(deliveryList);

            deliveryList = shopManager.CreateDeliveryList();
            deliveryList.Add(food, new ProductInfo(10, 20));
            deliveryList.Add(clothes, new ProductInfo(5, 10));
            dixi.AddProducts(deliveryList);

            var shoppingList = shopManager.CreateShoppingList();
            shoppingList.Add(education, 1);

            Assert.AreEqual(shopManager.FindCheapestShop(shoppingList), null);

            shoppingList = shopManager.CreateShoppingList();
            shoppingList.Add(food, 100);

            Assert.AreEqual(shopManager.FindCheapestShop(shoppingList), null);
        }

        [Test]
        [TestCase(10000, 30u, 5u, 3u)]
        public void PersonBuyProduct_MoneyAndProductCountChanged(int moneyBefore, uint productPrice, uint productCount, uint productToBuyCount)
        {
            var person = new Person("name", moneyBefore);
            var shopManager = new ShopManager();
            var shop = shopManager.Create("shop name", new Address("street", 10));
            var product = shopManager.RegisterProduct("product name");

            var deliveryList = shopManager.CreateDeliveryList();
            deliveryList.Add(product, new ProductInfo(productCount, productPrice));
            shop.AddProducts(deliveryList);


            var shoppingList = shopManager.CreateShoppingList();
            shoppingList.Add(product, productToBuyCount);
            shop.Buy(person, shoppingList);

            Assert.AreEqual(moneyBefore - productPrice * productToBuyCount, person.Money);
            Assert.AreEqual(productCount - productToBuyCount, shop.GetProductInfo(product).Count);
        }
    }
}
