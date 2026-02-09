using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class ContextMenu<T> : Popover<T> where T : ContextMenu<T>
    {
        public ContextMenu(ElementPortal portal, params IElement[] children) : base(portal, children)
        {
            VisualElement.AddToClassList("elements-context-menu");
            VisualElement.style.flexDirection = FlexDirection.Column;
        }

        public override VisualElement BuildVisualElement()
        {
            // Set parent menu reference on children
            foreach (var child in Children)
            {
                if (child is IMenuChild menuChild)
                {
                    menuChild.SetParentMenu(this);
                }
            }

            return base.BuildVisualElement();
        }

        public void CloseFromChild()
        {
            Close();
        }
    }

    public class ContextMenu : ContextMenu<ContextMenu>
    {
        public ContextMenu(ElementPortal portal, params IElement[] children) : base(portal, children)
        {
        }
    }

    public interface IMenuChild
    {
        void SetParentMenu<T>(ContextMenu<T> menu) where T : ContextMenu<T>;
    }
}
