using System;

namespace Shops.UI
{
    public class MenuAction<T>
    {
        private T _actionName;
        private Action _action;

        public MenuAction(T actionName, Action action)
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
            _action();
        }
    }
}
