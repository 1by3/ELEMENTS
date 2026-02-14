using System;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class MenuItem<T> : Button<T>, IMenuChild where T : MenuItem<T>
    {
        private Action parentCloseAction;

        public MenuItem(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("elements-menu-item");
            VisualElement.RegisterCallback<ClickEvent>(_ => parentCloseAction?.Invoke());
        }

        public void SetParentMenu<TMenu>(ContextMenu<TMenu> menu) where TMenu : ContextMenu<TMenu>
        {
            parentCloseAction = menu.CloseFromChild;
        }
    }

    public class MenuItem : MenuItem<MenuItem>
    {
        public MenuItem(params IElement[] children) : base(children) { }
    }
}
