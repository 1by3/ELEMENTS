using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Helpers
{
    public static class ElementsUI
    {
        public static bool IsUIFocused
        {
            get
            {
                foreach (var doc in Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None))
                {
                    var root = doc.rootVisualElement;
                    if (root?.focusController?.focusedElement != null) return true;
                }

                return false;
            }
        }

        public static bool IsUIHovered
        {
            get
            {
                foreach (var doc in Object.FindObjectsByType<UIDocument>(FindObjectsSortMode.None))
                {
                    var root = doc.rootVisualElement;
                    var panel = root?.panel;
                    if (panel == null) continue;

                    var panelPosition = RuntimePanelUtils.ScreenToPanel(panel, Input.mousePosition);
                    var pickedElement = panel.Pick(panelPosition);

                    if (pickedElement != null && pickedElement != root) return true;
                }

                return false;
            }
        }
    }
}
