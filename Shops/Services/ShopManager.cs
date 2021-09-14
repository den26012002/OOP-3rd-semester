using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Services
{
    public class ShopManager
    {
        private uint _nextShopId = 0;
        private uint _nextProductId = 0;
        public ShopManager()
        {
            Shops = new List<Shop>();
            Products = new List<Product>();
        }

        public List<Shop> Shops { get; }
        public List<Product> Products { get; }
        public Shop Create(string shopName, Address shopAddress)
        {
            var newShop = new Shop(shopName, shopAddress, _nextShopId++);
            Shops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string productName)
        {
            var newProduct = new Product(productName, _nextProductId++);
            Products.Add(newProduct);
            return newProduct;
        }

        public Dictionary<Product, ProductInfo> CreateDeliveryList()
        {
            return new Dictionary<Product, ProductInfo>();
        }

        public Dictionary<Product, uint> CreateShoppingList()
        {
            return new Dictionary<Product, uint>();
        }

        public Shop FindCheapestShop(Dictionary<Product, uint> shoppingList)
        {
            uint? minPurchasePrice = null;
            Shop cheapestShop = null;
            foreach (Shop shop in Shops)
            {
                uint? purchasePrice = 0;
                foreach (Product product in shoppingList.Keys)
                {
                    uint count = shoppingList[product];
                    if (!shop.HasProduct(product, count))
                    {
                        purchasePrice = null;
                        break;
                    }

                    purchasePrice += shop.GetProductInfo(product).Price * count;
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
