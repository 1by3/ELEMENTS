using ELEMENTS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    [RequireComponent(typeof(UIDocument))]
    public class ElementPortal : MonoBehaviour
    {
        public static ElementPortal Current { get; private set; }

        private UIDocument uiDocument;

        private void OnEnable()
        {
            Current = this;
            uiDocument = GetComponent<UIDocument>();
            uiDocument.rootVisualElement.pickingMode = PickingMode.Ignore;
            uiDocument.AddStyleSheet("ELEMENTS/DefaultStyles");
            uiDocument.AddStyleSheet("ELEMENTS/ExtendedStyles");
        }

        private void OnDisable()
        {
            if (Current == this) Current = null;
        }

        public void AddToPortal(VisualElement element)
        {
            uiDocument.rootVisualElement.Add(element);
        }

        public void RemoveFromPortal(VisualElement element)
        {
            if (!uiDocument.rootVisualElement.Contains(element)) return;
            uiDocument.rootVisualElement.Remove(element);
        }

        public Rect GetPortalBounds()
        {
            return uiDocument.rootVisualElement.worldBound;
        }

        public Vector2 ScreenToPanel(Vector2 screenPosition)
        {
            var panel = uiDocument.rootVisualElement.panel;
            return RuntimePanelUtils.ScreenToPanel(panel, screenPosition);
        }
    }
}
