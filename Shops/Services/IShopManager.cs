using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Services
{
    public interface IShopManager
    {
        IReadOnlyList<Shop> Shops { get; }
        IReadOnlyList<Product> Products { get; }
        Shop Create(string shopName, Address shopAddress);
        Product RegisterProduct(string productName);
        Shop FindCheapestShop(List<ProductRequest> shoppingList);
    }
}
