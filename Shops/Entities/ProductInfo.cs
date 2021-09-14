using System;

namespace Shops.Entities
{
    public class ProductInfo : ICloneable
    {
        public ProductInfo(uint count, uint price)
        {
            Count = count;
            Price = price;
        }

        public uint Count { get; set; }
        public uint Price { get; set; }

        public object Clone()
        {
            return new ProductInfo(Count, Price);
        }
    }
}
