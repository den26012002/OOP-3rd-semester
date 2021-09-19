using System.Collections.Generic;
using Shops.Entities;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager : IShopManager
    {
        private uint _nextShopId = 0;
        private uint _nextProductId = 0;
        private List<Shop> _shops;
        private List<Product> _products;
        public ShopManager()
        {
            _shops = new List<Shop>();
            _products = new List<Product>();
        }

        public IReadOnlyList<Shop> Shops { get => _shops; }
        public IReadOnlyList<Product> Products { get => _products; }
        public Shop Create(string shopName, Address shopAddress)
        {
            var newShop = new Shop(shopName, shopAddress, _nextShopId++);
            _shops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string productName)
        {
            var newProduct = new Product(productName, _nextProductId++);
            _products.Add(newProduct);
            return newProduct;
        }

        public Shop FindCheapestShop(List<ProductRequest> shoppingList)
        {
            uint? minPurchasePrice = null;
            Shop cheapestShop = null;
            foreach (Shop shop in Shops)
            {
                uint? purchasePrice = 0;
                try
                {
                    purchasePrice = shop.GetPrice(shoppingList);
                }
                catch (ShopsException)
                {
                    purchasePrice = null;
                }

                if (purchasePrice != null && (minPurchasePrice == null || purchasePrice < minPurchasePrice))
                {
                    minPurchasePrice = purchasePrice;
                    cheapestShop = shop;
                }
            }

            return cheapestShop;
        }
    }
}
