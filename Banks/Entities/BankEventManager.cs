using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class BankEventManager
    {
        private List<BankEventListener> _bankEventListeners;

        public BankEventManager()
        {
            _bankEventListeners = new List<BankEventListener>();
        }

        public void Subscribe(BankEventListener bankEventListener)
        {
            if (_bankEventListeners.Contains(bankEventListener))
            {
                throw new BanksException("Errror: the client has already subscribed");
            }

            _bankEventListeners.Add(bankEventListener);
        }

        public void Unsubscribe(BankEventListener bankEventListener)
        {
            _bankEventListeners.Remove(bankEventListener);
        }

        public void Notify(string conditionsMessage)
        {
            foreach (BankEventListener bankEventListener in _bankEventListeners)
            {
                bankEventListener.Update(conditionsMessage);
            }
        }
    }
}
