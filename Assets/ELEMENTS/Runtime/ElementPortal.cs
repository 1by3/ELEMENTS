using System;
using ELEMENTS.MVVM;
using ELEMENTS.Extensions;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class ElementPortal : MonoBehaviour
    {
        protected VisualElement ComponentRoot;
        protected VisualElement PortalRoot;

        protected UIDocument UIDocument;

        protected void Awake()
        {
            UIDocument = GetComponent<UIDocument>();
        }

        protected virtual void OnEnable()
        {
            UIDocument = GetComponent<UIDocument>();

            UIDocument.rootVisualElement.Clear();

            ComponentRoot = new VisualElement
            {
                name = "ComponentRoot",
                style =
                {
                    position = Position.Absolute,
                    flexGrow = 1,
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                    width = new StyleLength(new Length(100, LengthUnit.Percent))
                },
                pickingMode = PickingMode.Ignore
            };

            PortalRoot = new VisualElement
            {
                name = "PortalRoot",
                style =
                {
                    position = Position.Absolute,
                    flexGrow = 1,
                    height = new StyleLength(new Length(100, LengthUnit.Percent)),
                    width = new StyleLength(new Length(100, LengthUnit.Percent))
                },
                pickingMode = PickingMode.Ignore
            };

            UIDocument.rootVisualElement.Add(ComponentRoot);
            UIDocument.rootVisualElement.Add(PortalRoot);
        }

        public void AddToPortal(VisualElement element)
        {
            PortalRoot.Add(element);
        }

        public void RemoveFromPortal(VisualElement element)
        {
            if (!PortalRoot.Contains(element)) return;
            PortalRoot.Remove(element);
        }

        public (TViewModel, TView) RenderComponent<TViewModel, TView>(Action<TViewModel> configure = null)
            where TViewModel : ViewModel
            where TView : View<TViewModel>
        {
            return ComponentRoot.RenderComponent<TViewModel, TView>(configure);
        }

        public void StyleSheet(StyleSheet styleSheet)
        {
            UIDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }

        public void StyleSheet(string styleSheetPath)
        {
            var styleSheet = Resources.Load<StyleSheet>(styleSheetPath);
            if (styleSheet == null) throw new Exception($"StyleSheet not found at path: {styleSheetPath}");
            StyleSheet(styleSheet);
        }
    }
}