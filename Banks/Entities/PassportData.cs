namespace Banks.Entities
{
    public class PassportData
    {
        public PassportData(uint series, uint number)
        {
            Series = series;
            Number = number;
        }

        public uint Series { get; }
        public uint Number { get; }
    }
}
