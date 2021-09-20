namespace Shops.Entities
{
    public class ProductInfo
    {
        public ProductInfo(Product product, uint count, uint price)
        {
            Product = product;
            Count = count;
            Price = price;
        }

        public Product Product { get; }
        public uint Count { get; }
        public uint Price { get; }
    }
}
