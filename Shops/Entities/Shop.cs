using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        public Shop(string name, Address address, uint id)
        {
            Name = name;
            Address = address;
            Id = id;
            ProductList = new Dictionary<Product, ProductInfo>();
        }

        public uint Id { get; }
        public string Name { get; }
        public Address Address { get; }
        private Dictionary<Product, ProductInfo> ProductList { get; }
        public void AddProducts(Dictionary<Product, ProductInfo> deliveryList)
        {
            foreach (Product product in deliveryList.Keys)
            {
                ProductInfo productInfo = deliveryList[product];
                if (!ProductList.ContainsKey(product))
                {
                    ProductList.Add(product, new ProductInfo(0, 0));
                }

                ProductList[product] = new ProductInfo(ProductList[product].Count + productInfo.Count, productInfo.Price);
            }
        }

        public void Buy(Person customer, Dictionary<Product, uint> shoppingList)
        {
            uint purchasePrice = 0;
            foreach (Product product in shoppingList.Keys)
            {
                if (!HasProduct(product))
                {
                    throw new ShopsException("Error: there is no product with name: " + product.Name);
                }

                if (!HasProduct(product, shoppingList[product]))
                {
                    throw new ShopsException("Error: there is not enought products with name: " + product.Name);
                }

                purchasePrice += ProductList[product].Price * shoppingList[product];
            }

            if (customer.Money - purchasePrice < 0)
            {
                throw new ShopsException("Error: not enough money");
            }

            customer.SpendMoney(purchasePrice);

            foreach (Product product in shoppingList.Keys)
            {
                ProductList[product].Count -= shoppingList[product];
            }
        }

        public ProductInfo GetProductInfo(Product product)
        {
            if (!HasProduct(product))
            {
                throw new ShopsException($"Error: there is no product with name: \"{product.Name}\" in shop with name \"{Name}\"");
            }

            return (ProductInfo)ProductList[product].Clone();
        }

        public void UpdatePrice(Product product, uint newPrice)
        {
            if (!HasProduct(product))
            {
                throw new ShopsException($"Error: there is no product with name \"{product.Name}\" in shop with name \"{Name}\"");
            }

            ProductList[product].Price = newPrice;
        }

        public bool HasProduct(Product product)
        {
            return ProductList.ContainsKey(product);
        }

        public bool HasProduct(Product product, uint count)
        {
            return HasProduct(product) && ProductList[product].Count >= count;
        }
    }
}
