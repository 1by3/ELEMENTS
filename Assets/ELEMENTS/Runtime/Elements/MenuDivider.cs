namespace ELEMENTS.Elements
{
    public class MenuDivider<T> : BaseElement<T>, IMenuChild where T : MenuDivider<T>
    {
        public MenuDivider()
        {
            VisualElement.AddToClassList("elements-menu-divider");
        }

        public void SetParentMenu<TMenu>(ContextMenu<TMenu> menu) where TMenu : ContextMenu<TMenu>
        {
            // No-op: dividers don't need parent menu reference
        }
    }

    public class MenuDivider : MenuDivider<MenuDivider>
    {
        public MenuDivider() : base() { }
    }
}
