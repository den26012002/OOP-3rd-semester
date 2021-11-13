namespace Banks.Entities
{
    public class DepositFee
    {
        public DepositFee(int limit, double percents)
        {
            Limit = limit;
            Percents = percents;
        }

        public int Id { get; private init; }
        public int Limit { get; private init; }
        public double Percents { get; private init; }
    }
}
