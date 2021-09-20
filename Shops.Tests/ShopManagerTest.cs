using System.Collections.Generic;
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

            var deliveryList = new List<ProductInfo>();
            deliveryList.Add(new ProductInfo(product, productCount, 100));
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

            var deliveryList = new List<ProductInfo>();
            deliveryList.Add(new ProductInfo(product, 10, startPrice));
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

            var deliveryList = new List<ProductInfo>();
            deliveryList.Add(new ProductInfo(food, 15, 20));
            deliveryList.Add(new ProductInfo(clothes, 10, 10));
            pyaterochka.AddProducts(deliveryList);

            deliveryList = new List<ProductInfo>();
            deliveryList.Add(new ProductInfo(food, 10, 20));
            deliveryList.Add(new ProductInfo(clothes, 5, 10));
            dixi.AddProducts(deliveryList);

            var shoppingList = new List<ProductRequest>();
            shoppingList.Add(new ProductRequest(education, 1));

            Assert.AreEqual(shopManager.FindCheapestShop(shoppingList), null);

            shoppingList = new List<ProductRequest>();
            shoppingList.Add(new ProductRequest(food, 100));

            Assert.AreEqual(shopManager.FindCheapestShop(shoppingList), null);
        }

        [Test]
        [TestCase(10000u, 30u, 5u, 3u)]
        public void PersonBuyProduct_MoneyAndProductCountChanged(uint moneyBefore, uint productPrice, uint productCount, uint productToBuyCount)
        {
            var person = new Person("name", moneyBefore);
            var shopManager = new ShopManager();
            var shop = shopManager.Create("shop name", new Address("street", 10));
            var product = shopManager.RegisterProduct("product name");

            var deliveryList = new List<ProductInfo>();
            deliveryList.Add(new ProductInfo(product, productCount, productPrice));
            shop.AddProducts(deliveryList);


            var shoppingList = new List<ProductRequest>();
            shoppingList.Add(new ProductRequest(product, productToBuyCount));
            shop.Buy(person, shoppingList);

            Assert.AreEqual(moneyBefore - productPrice * productToBuyCount, person.Money);
            Assert.AreEqual(productCount - productToBuyCount, shop.GetProductInfo(product).Count);
        }
    }
}
