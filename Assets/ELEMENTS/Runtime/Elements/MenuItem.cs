using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class MenuItem<T> : BaseElement<T>, IMenuChild where T : MenuItem<T>
    {
        private Action parentCloseAction;
        private Action onClick;
        private UnityEngine.UIElements.Image iconElement;
        private UnityEngine.UIElements.Label labelElement;

        public MenuItem(string label, Action onClick = null)
        {
            VisualElement.AddToClassList("elements-menu-item");

            labelElement = new UnityEngine.UIElements.Label(label);
            VisualElement.Add(labelElement);

            this.onClick = onClick;

            VisualElement.RegisterCallback<ClickEvent>(HandleClick);
        }

        public MenuItem(Texture2D icon, string label, Action onClick = null) : this(label, onClick)
        {
            SetIconInternal(icon);
        }

        public MenuItem(string iconPath, string label, Action onClick = null) : this(label, onClick)
        {
            var texture = Resources.Load<Texture2D>(iconPath);
            if (texture != null)
            {
                SetIconInternal(texture);
            }
        }

        private void SetIconInternal(Texture2D icon)
        {
            if (iconElement == null)
            {
                iconElement = new UnityEngine.UIElements.Image { image = icon };
                VisualElement.Insert(0, iconElement);
            }
            else
            {
                iconElement.image = icon;
            }
        }

        public T Icon(Texture2D icon)
        {
            SetIconInternal(icon);
            return (T)this;
        }

        public T Icon(string iconPath)
        {
            var texture = Resources.Load<Texture2D>(iconPath);
            if (texture != null)
            {
                SetIconInternal(texture);
            }

            return (T)this;
        }

        private void HandleClick(ClickEvent evt)
        {
            if (GetDisabled()) return;

            onClick?.Invoke();
            parentCloseAction?.Invoke();
        }

        public void SetParentMenu<TMenu>(ContextMenu<TMenu> menu) where TMenu : ContextMenu<TMenu>
        {
            parentCloseAction = menu.CloseFromChild;
        }

        public T Label(string label)
        {
            labelElement.text = label;
            return (T)this;
        }

        public string GetLabel()
        {
            return labelElement.text;
        }
    }

    public class MenuItem : MenuItem<MenuItem>
    {
        public MenuItem(string label, Action onClick = null) : base(label, onClick) { }
        public MenuItem(Texture2D icon, string label, Action onClick = null) : base(icon, label, onClick) { }
        public MenuItem(string iconPath, string label, Action onClick = null) : base(iconPath, label, onClick) { }
    }
}
