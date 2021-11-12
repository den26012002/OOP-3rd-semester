using System;

namespace Banks.UI
{
    public class BanksUIMenuAction
    {
        private string _actionName;
        private Action _action;

        public BanksUIMenuAction(string actionName, Action action)
        {
            _actionName = actionName;
            _action = action;
        }

        public override string ToString()
        {
            return _actionName.ToString();
        }

        public void Invoke()
        {
            _action.Invoke();
        }
    }
}
