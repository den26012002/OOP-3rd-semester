using System.Collections.Generic;

namespace Banks.Entities
{
    public class BankConditions
    {
        private List<DepositFee> _depositFees;

        public BankConditions(uint creditFee, List<DepositFee> depositFees, int shadyCustomersLimit)
        {
            ShadyCustomersLimit = shadyCustomersLimit;
            CreditFee = creditFee;
            _depositFees = depositFees;
        }

        private BankConditions()
        {
        }

        public int Id { get; private init; }
        public int ShadyCustomersLimit { get; private init; }
        public uint CreditFee { get; private init; }
        public IReadOnlyList<DepositFee> DepositFees => _depositFees;

        public override string ToString()
        {
            string resultString = $"Shady customers limit: {ShadyCustomersLimit}\nCredit Fee: {CreditFee}\nDeposit fees:\n";
            foreach (DepositFee fee in _depositFees)
            {
                resultString += $"Upper than {fee.Limit}: {fee.Percents}%\n";
            }

            return resultString;
        }
    }
}
