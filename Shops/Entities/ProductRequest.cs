namespace Shops.Entities
{
    public class ProductRequest
    {
        public ProductRequest(Product product, uint count)
        {
            Product = product;
            Count = count;
        }

        public Product Product { get; }
        public uint Count { get; }
    }
}
