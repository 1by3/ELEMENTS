using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Scripts.Elements
{
    public abstract class BaseElement<T> : IDisposable, IElement where T : IElement
    {
        protected VisualElement VisualElement = new();
        protected readonly List<IDisposable> Disposables = new();
        protected bool RendersInPortal = false;

        protected event EventHandler<float> OnUpdate
        {
            add
            {
                if (ElementPortal.Instance == null)
                {
                    throw new Exception("No ElementPortal present in the scene. Please add one to the scene before using elements with Update handlers.");
                }
                ElementPortal.Instance.OnUpdate += value;
            }
            remove
            {
                if (ElementPortal.Instance == null)
                {
                    throw new Exception("No ElementPortal present in the scene. Please add one to the scene before using elements with Update handlers.");
                }
                ElementPortal.Instance.OnUpdate -= value;
            }
        }

        public T Ref(ref T bound)
        {
            bound = (T)(object)this;
            return (T)(object)this;
        }

        public T Name(string name)
        {
            VisualElement.name = name;
            return (T)(object)this;
        }

        public string GetName()
        {
            return VisualElement.name;
        }

        public T BindName(Observable<string> name)
        {
            Disposables.Add(name.Subscribe(nv => Name(nv)));
            return (T)(object)this;
        }

        public T Disabled(bool disabled = true)
        {
            VisualElement.SetEnabled(!disabled);
            return (T)(object)this;
        }

        public bool GetDisabled()
        {
            return !VisualElement.enabledSelf;
        }

        public T BindDisabled(Observable<bool> disabled)
        {
            Disposables.Add(disabled.Subscribe(v => Disabled(v)));
            return (T)(object)this;
        }

        public T PickingMode(PickingMode pickingMode)
        {
            VisualElement.pickingMode = pickingMode;
            return (T)(object)this;
        }

        public PickingMode GetPickingMode()
        {
            return VisualElement.pickingMode;
        }

        public T BindPickingMode(Observable<PickingMode> pickingMode)
        {
            Disposables.Add(pickingMode.Subscribe(v => PickingMode(v)));
            return (T)(object)this;
        }

        public T StyleSheet(StyleSheet styleSheet)
        {
            VisualElement.styleSheets.Add(styleSheet);
            return (T)(object)this;
        }

        public T StyleSheet(string styleSheetPath)
        {
            var styleSheet = Resources.Load<StyleSheet>(styleSheetPath);
            if (styleSheet == null) throw new Exception($"StyleSheet not found at path: {styleSheetPath}");
            return StyleSheet(styleSheet);
        }

        public T ClassName(string className, bool enabled = true)
        {
            if (enabled)
            {
                VisualElement.AddToClassList(className);
            }
            else
            {
                VisualElement.RemoveFromClassList(className);
            }

            return (T)(object)this;
        }

        public bool HasClassName(string className)
        {
            return VisualElement.ClassListContains(className);
        }

        public T BindClassName(string className, Observable<bool> enabled)
        {
            Disposables.Add(enabled.Subscribe(v => ClassName(className, v)));
            return (T)(object)this;
        }

        public T Visible(bool visible)
        {
            VisualElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            return (T)(object)this;
        }

        public bool GetVisible()
        {
            return VisualElement.style.display == DisplayStyle.Flex;
        }

        public T BindVisible(Observable<bool> visible)
        {
            Disposables.Add(visible.Subscribe(v => Visible(v)));
            return (T)(object)this;
        }

        public T RegisterCallback<TEvent>(EventCallback<TEvent> callback) where TEvent : EventBase<TEvent>, new()
        {
            VisualElement.RegisterCallback(callback);
            return (T)(object)this;
        }

        public T OnClick(EventCallback<ClickEvent> handler)
        {
            return RegisterCallback(handler);
        }

        public virtual void Dispose()
        {
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }

            Disposables.Clear();

            if (VisualElement == null)
            {
                return;
            }

            VisualElement.RemoveFromHierarchy();
            VisualElement = null;
        }

        public virtual VisualElement BuildVisualElement()
        {
            if (!RendersInPortal)
            {
                return VisualElement;
            }

            if (ElementPortal.Instance == null)
            {
                throw new Exception("No ElementPortal present in the scene. Please add one to the scene before using portal-based elements.");
            }

            ElementPortal.Instance.RemoveFromPortal(VisualElement);
            ElementPortal.Instance.AddToPortal(VisualElement);

            return null;
        }

        public T Padding(int paddingTop, int paddingRight, int paddingBottom, int paddingLeft)
        {
            VisualElement.style.paddingTop = paddingTop;
            VisualElement.style.paddingRight = paddingRight;
            VisualElement.style.paddingBottom = paddingBottom;
            VisualElement.style.paddingLeft = paddingLeft;
            return (T)(object)this;
        }


        public T Padding(int padding)
        {
            return Padding(padding, padding, padding, padding);
        }

        public T PaddingVertical(int padding)
        {
            VisualElement.style.paddingTop = padding;
            VisualElement.style.paddingBottom = padding;
            return (T)(object)this;
        }

        public T PaddingHorizontal(int padding)
        {
            VisualElement.style.paddingLeft = padding;
            VisualElement.style.paddingRight = padding;
            return (T)(object)this;
        }

        public T Margin(int marginTop, int marginRight, int marginBottom, int marginLeft)
        {
            VisualElement.style.marginTop = marginTop;
            VisualElement.style.marginRight = marginRight;
            VisualElement.style.marginBottom = marginBottom;
            VisualElement.style.marginLeft = marginLeft;
            return (T)(object)this;
        }

        public T Margin(int margin)
        {
            return Padding(margin, margin, margin, margin);
        }

        public T MarginVertical(int margin)
        {
            VisualElement.style.marginTop = margin;
            VisualElement.style.marginBottom = margin;
            return (T)(object)this;
        }

        public T MarginHorizontal(int margin)
        {
            VisualElement.style.marginLeft = margin;
            VisualElement.style.marginRight = margin;
            return (T)(object)this;
        }
    }
}