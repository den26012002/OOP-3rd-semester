namespace Banks.Entities
{
    public class PassportData
    {
        public PassportData(uint series, uint number)
        {
            Series = series;
            Number = number;
        }

        public int Id { get; private init; }
        public uint Series { get; private init; }
        public uint Number { get; private init; }
    }
}
