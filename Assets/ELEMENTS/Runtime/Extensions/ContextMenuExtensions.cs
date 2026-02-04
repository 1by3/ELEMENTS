using ELEMENTS.Elements;
using UnityEngine;
using UnityEngine.UIElements;
using ContextMenu = ELEMENTS.Elements.ContextMenu;

namespace ELEMENTS.Extensions
{
    public static class ContextMenuExtensions
    {
        /// <summary>
        /// Opens the context menu on left-click, anchored below the element.
        /// </summary>
        public static T WithContextMenu<T>(this T element, ContextMenu menu) where T : BaseElement<T>
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
        /// Opens the context menu on right-click at the cursor position.
        /// </summary>
        public static T WithRightClickMenu<T>(this T element, ContextMenu menu) where T : BaseElement<T>
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
