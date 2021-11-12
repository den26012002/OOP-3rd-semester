using System.Collections.Generic;

namespace Banks.Entities
{
    public class BankEventListener
    {
        private List<string> _conditionsMessages;

        public BankEventListener()
        {
            _conditionsMessages = new List<string>();
        }

        public IReadOnlyList<string> ConditionsMessages { get; }
        internal void Update(string conditionsMessage)
        {
            _conditionsMessages.Add(conditionsMessage);
        }
    }
}
