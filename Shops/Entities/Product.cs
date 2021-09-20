namespace Shops.Entities
{
    public class Product
    {
        public Product(string name, uint id)
        {
            Name = name;
            Id = id;
        }

        public uint Id { get; }
        public string Name { get; }
    }
}
