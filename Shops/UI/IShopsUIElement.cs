using System.Collections.Generic;
using Shops.Services;

namespace Shops.UI
{
    public interface IShopsUIElement
    {
        IShopUIManager Manager { get; }
        IShopsUIElement ParentElement { get; }
        List<IShopsUIElement> ChildElements { get; }
        void Show();
        void AddChildElement(IShopsUIElement childElement);

        void RemoveChildElement(IShopsUIElement childElement);

        void ChangeParent(IShopsUIElement newParentElement);
    }
}
