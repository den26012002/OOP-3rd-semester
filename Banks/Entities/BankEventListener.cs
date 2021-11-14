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

        public int Id { get; private init; }
        public IReadOnlyList<string> ConditionsMessages => _conditionsMessages;
        internal void Update(string conditionsMessage)
        {
            _conditionsMessages.Add(conditionsMessage);
        }
    }
}
