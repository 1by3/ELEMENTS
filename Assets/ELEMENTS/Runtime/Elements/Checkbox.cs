using System;
using System.Collections.Generic;
using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class Checkbox<T> : BaseElement<T> where T : Checkbox<T>
    {
        private bool _value;
        private readonly List<Action<bool>> _changeHandlers = new();
        private readonly VisualElement _box;
        private readonly UnityEngine.UIElements.Label _check;
        private UnityEngine.UIElements.Label _label;

        public Checkbox()
        {
            VisualElement.AddToClassList("elements-checkbox");

            _box = new VisualElement();
            _box.AddToClassList("checkbox-box");

            _check = new UnityEngine.UIElements.Label("\u2713");
            _check.AddToClassList("checkbox-check");

            _box.Add(_check);
            VisualElement.Add(_box);

            VisualElement.RegisterCallback<ClickEvent>(evt =>
            {
                if (GetDisabled()) return;
                _value = !_value;
                VisualElement.EnableInClassList("checked", _value);
                foreach (var handler in _changeHandlers)
                    handler(_value);
            });
        }

        public Checkbox(ReactiveProperty<bool> value) : this()
        {
            BindValue(value);
        }

        public T Value(bool value)
        {
            _value = value;
            VisualElement.EnableInClassList("checked", _value);
            return (T)this;
        }

        public bool GetValue()
        {
            return _value;
        }

        public T BindValue(ReactiveProperty<bool> value)
        {
            OnChange(newValue => value.Value = newValue);
            Disposables.Add(value.Subscribe(nv => Value(nv)));
            return (T)this;
        }

        public T Label(string label)
        {
            if (_label == null)
            {
                _label = new UnityEngine.UIElements.Label();
                _label.AddToClassList("checkbox-label");
                VisualElement.Add(_label);
            }

            _label.text = label;
            return (T)this;
        }

        public string GetLabel()
        {
            return _label?.text;
        }

        public T BindLabel(Observable<string> label)
        {
            Disposables.Add(label.Subscribe(nv => Label(nv)));
            return (T)this;
        }

        public T OnChange(Action<bool> handler)
        {
            _changeHandlers.Add(handler);
            return (T)this;
        }
    }

    public class Checkbox : Checkbox<Checkbox>
    {
        public Checkbox() : base() { }
        public Checkbox(ReactiveProperty<bool> value) : base(value) { }
    }
}
