using ELEMENTS.Elements;
using UnityEngine.UIElements;

namespace ELEMENTS.Extensions
{
    public static class PopoverExtensions
    {
        /// <summary>
        /// Opens the popover on left-click, anchored below the element.
        /// </summary>
        public static T WithContextMenu<T, TPopover>(this T element, Popover<TPopover> menu) where T : BaseElement<T> where TPopover : Popover<TPopover>
        {
            element.RegisterCallback<ClickEvent>(evt =>
            {
                var ve = element.GetVisualElement();
                menu.AnchorTo(ve, AnchorPosition.Below);
                menu.BuildVisualElement();
                menu.Open();
            });

            return element;
        }

        /// <summary>
        /// Opens the popover on right-click at the cursor position.
        /// </summary>
        public static T WithRightClickMenu<T, TPopover>(this T element, Popover<TPopover> menu) where T : BaseElement<T> where TPopover : Popover<TPopover>
        {
            element.RegisterCallback<PointerDownEvent>(evt =>
            {
                if (evt.button != 1) return; // Right click

                menu.Position(evt.position.x, evt.position.y);
                menu.BuildVisualElement();
                menu.Open();
                evt.StopPropagation();
            });

            return element;
        }
    }
}
