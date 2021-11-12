namespace Banks.Entities
{
    public class DepositFee
    {
        public DepositFee(int limit, double percents)
        {
            Limit = limit;
            Percents = percents;
        }

        public int Limit { get; }
        public double Percents { get; }
    }
}
