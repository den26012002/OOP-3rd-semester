namespace Shops.Entities
{
    public class Person
    {
        public Person(string name, int money)
        {
            Name = name;
            Money = money;
        }

        public string Name { get; }
        public int Money { get; private set; } // пусть можно брать в долг

        public void SpendMoney(uint amount)
        {
            Money -= (int)amount;
        }

        public void EarnMoney(uint amount)
        {
            Money += (int)amount;
        }
    }
}
