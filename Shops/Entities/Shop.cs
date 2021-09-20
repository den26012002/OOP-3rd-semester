using System.Collections.Generic;
using Shops.Tools;

namespace Shops.Entities
{
    public class Shop
    {
        private List<ProductInfo> _productList;
        public Shop(string name, Address address, uint id)
        {
            Name = name;
            Address = address;
            Id = id;
            _productList = new List<ProductInfo>();
        }

        public uint Id { get; }
        public string Name { get; }
        public Address Address { get; }
        private IReadOnlyList<ProductInfo> ProductList { get => _productList; }
        public void AddProducts(List<ProductInfo> deliveryList)
        {
            foreach (ProductInfo deliveryProductInfo in deliveryList)
            {
                ProductInfo oldProductInfo = _productList.Find(productInfo => productInfo.Product == deliveryProductInfo.Product);
                if (oldProductInfo == null)
                {
                    _productList.Add(deliveryProductInfo);
                }
                else
                {
                    oldProductInfo = new ProductInfo(oldProductInfo.Product, oldProductInfo.Count + deliveryProductInfo.Count, deliveryProductInfo.Price);
                }
            }
        }

        public uint GetPrice(List<ProductRequest> shoppingList)
        {
            uint purchasePrice = 0;
            foreach (ProductRequest productRequest in shoppingList)
            {
                Product product = productRequest.Product;
                if (!HasProduct(product))
                {
                    throw new ShopsException("Error: there is no product with name: " + product.Name);
                }

                if (!HasProduct(product, productRequest.Count))
                {
                    throw new ShopsException("Error: there is not enought products with name: " + product.Name);
                }

                purchasePrice += _productList.Find(productInfo => productInfo.Product == product).Price * productRequest.Count;
            }

            return purchasePrice;
        }

        public void Buy(Person customer, List<ProductRequest> shoppingList)
        {
            uint purchasePrice = GetPrice(shoppingList);

            if (customer.Money - purchasePrice < 0)
            {
                throw new ShopsException("Error: not enough money");
            }

            customer.SpendMoney(purchasePrice);

            foreach (ProductRequest productRequest in shoppingList)
            {
                Product product = productRequest.Product;
                ProductInfo productInfo = _productList.Find(productInfo => productInfo.Product == product);
                var newProductInfo = new ProductInfo(productInfo.Product, productInfo.Count - productRequest.Count, productInfo.Price);
                _productList.Remove(productInfo);
                _productList.Add(newProductInfo);
            }
        }

        public ProductInfo GetProductInfo(Product product)
        {
            if (!HasProduct(product))
            {
                throw new ShopsException($"Error: there is no product with name: \"{product.Name}\" in shop with name \"{Name}\"");
            }

            return _productList.Find(productInfo => productInfo.Product == product);
        }

        public void UpdatePrice(Product product, uint newPrice)
        {
            if (!HasProduct(product))
            {
                throw new ShopsException($"Error: there is no product with name \"{product.Name}\" in shop with name \"{Name}\"");
            }

            ProductInfo productInfo = _productList.Find(productInfo => productInfo.Product == product);
            var newProductInfo = new ProductInfo(productInfo.Product, productInfo.Count, newPrice);
            _productList.Remove(productInfo);
            _productList.Add(newProductInfo);
        }

        public bool HasProduct(Product product)
        {
            return _productList.Find(productInfo => productInfo.Product == product) != null;
        }

        public bool HasProduct(Product product, uint count)
        {
            return HasProduct(product) && _productList.Find(productInfo => productInfo.Product == product).Count >= count;
        }
    }
}
