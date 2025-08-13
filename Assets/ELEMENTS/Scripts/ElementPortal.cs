using System;
using ELEMENTS.Scripts.Extensions;
using ELEMENTS.Scripts.MVVM;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Scripts
{
    public class ElementPortal : MonoBehaviour
    {
        public static ElementPortal Instance { get; private set; }

        public EventHandler<float> OnUpdate { get; set; }

        protected VisualElement ComponentRoot;
        protected VisualElement PortalRoot;

        [SerializeField] protected UIDocument uiDocument;

        protected void Awake()
        {
            Instance = this;
        }

        protected virtual void OnEnable()
        {
            Instance = this;

            uiDocument.rootVisualElement.Clear();

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

            uiDocument.rootVisualElement.Add(ComponentRoot);
            uiDocument.rootVisualElement.Add(PortalRoot);
        }

        protected virtual void Update()
        {
            OnUpdate?.Invoke(this, Time.deltaTime);
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
            uiDocument.rootVisualElement.styleSheets.Add(styleSheet);
        }

        public void StyleSheet(string styleSheetPath)
        {
            var styleSheet = Resources.Load<StyleSheet>(styleSheetPath);
            if (styleSheet == null) throw new Exception($"StyleSheet not found at path: {styleSheetPath}");
            StyleSheet(styleSheet);
        }
    }
}