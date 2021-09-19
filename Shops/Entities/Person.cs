using Shops.Tools;

namespace Shops.Entities
{
    public class Person
    {
        public Person(string name, uint money)
        {
            Name = name;
            Money = money;
        }

        public string Name { get; }
        public uint Money { get; private set; }

        public void SpendMoney(uint amount)
        {
            if (amount > Money)
            {
                throw new ShopsException($"Error: person {Name} has not enough money");
            }

            Money -= amount;
        }

        public void EarnMoney(uint amount)
        {
            Money += amount;
        }
    }
}
