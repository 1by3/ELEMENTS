using System;
using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class TextField<T> : BaseElement<T> where T : TextField<T>
    {
        private string placeholderText = "";
        private readonly string placeholderClass = UnityEngine.UIElements.TextField.ussClassName + "__placeholder";
        private bool _placeholderRegistered;

        public TextField()
        {
            VisualElement = new UnityEngine.UIElements.TextField();
        }

        public TextField(ReactiveProperty<string> text) : this()
        {
            BindText(text);
        }

        public T Text(string text)
        {
            ((UnityEngine.UIElements.TextField)VisualElement).value = text;
            return (T)this;
        }

        public string GetText()
        {
            return ((UnityEngine.UIElements.TextField)VisualElement).value;
        }

        public T BindText(ReactiveProperty<string> text)
        {
            OnChange(newValue => text.Value = newValue);
            Disposables.Add(text.Subscribe(nv => Text(nv)));
            return (T)this;
        }

        public T Placeholder(string placeholder)
        {
            placeholderText = placeholder;

            if (!_placeholderRegistered)
            {
                _placeholderRegistered = true;
                ((UnityEngine.UIElements.TextField)VisualElement).RegisterCallback<FocusInEvent>(_ => OnFocusIn());
                ((UnityEngine.UIElements.TextField)VisualElement).RegisterCallback<FocusOutEvent>(_ => OnFocusOut());
            }

            OnFocusOut();

            return (T)this;
        }

        public string GetPlaceholder()
        {
            return placeholderText;
        }

        public T BindPlaceholder(Observable<string> placeholder)
        {
            Disposables.Add(placeholder.Subscribe(nv => Placeholder(nv)));
            return (T)this;
        }

        public T Password(bool password = true)
        {
            ((UnityEngine.UIElements.TextField)VisualElement).isPasswordField = password;
            return (T)this;
        }

        public bool GetPassword()
        {
            return ((UnityEngine.UIElements.TextField)VisualElement).isPasswordField;
        }

        public T BindPassword(Observable<bool> password)
        {
            Disposables.Add(password.Subscribe(nv => Password(nv)));
            return (T)this;
        }

        public T Multiline(bool multiline = true)
        {
            ((UnityEngine.UIElements.TextField)VisualElement).multiline = multiline;
            return (T)this;
        }

        public bool GetMultiline()
        {
            return ((UnityEngine.UIElements.TextField)VisualElement).multiline;
        }

        public T BindMultiline(Observable<bool> multiline)
        {
            Disposables.Add(multiline.Subscribe(nv => Multiline(nv)));
            return (T)this;
        }

        public T ReadOnly(bool readOnly = true)
        {
            ((UnityEngine.UIElements.TextField)VisualElement).isReadOnly = readOnly;
            return (T)this;
        }

        public bool GetReadOnly()
        {
            return ((UnityEngine.UIElements.TextField)VisualElement).isReadOnly;
        }

        public T BindReadOnly(Observable<bool> readOnly)
        {
            Disposables.Add(readOnly.Subscribe(nv => ReadOnly(nv)));
            return (T)this;
        }

        public T OnChange(Action<string> handler)
        {
            ((UnityEngine.UIElements.TextField)VisualElement).RegisterValueChangedCallback(evt => handler(evt.newValue));
            return (T)this;
        }

        private void OnFocusIn()
        {
            if (!((UnityEngine.UIElements.TextField)VisualElement).ClassListContains(placeholderClass)) return;
            ((UnityEngine.UIElements.TextField)VisualElement).value = string.Empty;
            ((UnityEngine.UIElements.TextField)VisualElement).RemoveFromClassList(placeholderClass);
        }

        private void OnFocusOut()
        {
            if (!string.IsNullOrEmpty(((UnityEngine.UIElements.TextField)VisualElement).text)) return;
            ((UnityEngine.UIElements.TextField)VisualElement).SetValueWithoutNotify(placeholderText);
            ((UnityEngine.UIElements.TextField)VisualElement).AddToClassList(placeholderClass);
        }
    }

    public class TextField : TextField<TextField>
    {
        public TextField() : base() { }
        public TextField(ReactiveProperty<string> text) : base(text) { }
    }
}