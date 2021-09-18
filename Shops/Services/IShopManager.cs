using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Services
{
    public interface IShopManager
    {
        List<Shop> Shops { get; }
        List<Product> Products { get; }
        Shop Create(string shopName, Address shopAddress);
        Product RegisterProduct(string productName);
        Dictionary<Product, ProductInfo> CreateDeliveryList();
        Dictionary<Product, uint> CreateShoppingList();
        Shop FindCheapestShop(Dictionary<Product, uint> shoppingList);
    }
}
