using System.Collections.Generic;
using Shops.Services;

namespace Shops.UI
{
    public abstract class BaseShopsUIMenu<T> : IShopsUIElement
    {
        protected BaseShopsUIMenu(IShopUIManager manager)
        {
            Manager = manager;
            ParentElement = null;
            ChildElements = new List<IShopsUIElement>();
        }

        public IShopUIManager Manager { get; }
        public IShopsUIElement ParentElement { get; private set; }
        public List<IShopsUIElement> ChildElements { get; }
        public virtual void Show()
        {
        }

        public void AddChildElement(IShopsUIElement childElement)
        {
            if (!ChildElements.Contains(childElement))
            {
                ChildElements.Add(childElement);
                childElement.ChangeParent(this);
            }
        }

        public void RemoveChildElement(IShopsUIElement childElement)
        {
            if (ChildElements.Contains(childElement))
            {
                ChildElements.Remove(childElement);
                childElement.ChangeParent(null);
            }
        }

        public void ChangeParent(IShopsUIElement newParentElement)
        {
            ParentElement?.RemoveChildElement(this);
            newParentElement?.AddChildElement(this);
            ParentElement = newParentElement;
        }

        public abstract void AddPreSelectionAction(MenuAction<T> action);
        public abstract void AddSelectionAction(MenuAction<T> action);
        public abstract void AddPostSelectionAction(MenuAction<T> action);

        public abstract void ClearActions();
    }
}
