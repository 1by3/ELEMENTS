using System;
using System.Collections.Generic;
using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class Checkbox<T> : BaseElement<T> where T : Checkbox<T>
    {
        private bool value;
        private readonly List<Action<bool>> changeHandlers = new();
        private readonly VisualElement box;
        private readonly UnityEngine.UIElements.Label check;
        private UnityEngine.UIElements.Label label;

        public Checkbox()
        {
            VisualElement.AddToClassList("elements-checkbox");

            box = new VisualElement();
            box.AddToClassList("checkbox-box");

            check = new UnityEngine.UIElements.Label("\u2713");
            check.AddToClassList("checkbox-check");

            box.Add(check);
            VisualElement.Add(box);

            VisualElement.RegisterCallback<ClickEvent>(evt =>
            {
                if (GetDisabled()) return;
                value = !value;
                VisualElement.EnableInClassList("checked", value);
                foreach (var handler in changeHandlers)
                    handler(value);
            });
        }

        public Checkbox(ReactiveProperty<bool> value) : this()
        {
            BindValue(value);
        }

        public T Value(bool value)
        {
            this.value = value;
            VisualElement.EnableInClassList("checked", this.value);
            return (T)this;
        }

        public bool GetValue()
        {
            return value;
        }

        public T BindValue(ReactiveProperty<bool> value)
        {
            OnChange(newValue => value.Value = newValue);
            Disposables.Add(value.Subscribe(nv => Value(nv)));
            return (T)this;
        }

        public T Label(string label)
        {
            if (this.label == null)
            {
                this.label = new UnityEngine.UIElements.Label();
                this.label.AddToClassList("checkbox-label");
                VisualElement.Add(this.label);
            }

            this.label.text = label;
            return (T)this;
        }

        public string GetLabel()
        {
            return label?.text;
        }

        public T BindLabel(Observable<string> label)
        {
            Disposables.Add(label.Subscribe(nv => Label(nv)));
            return (T)this;
        }

        public T OnChange(Action<bool> handler)
        {
            changeHandlers.Add(handler);
            return (T)this;
        }

        public override void Dispose()
        {
            changeHandlers.Clear();
            base.Dispose();
        }
    }

    public class Checkbox : Checkbox<Checkbox>
    {
        public Checkbox() : base() { }
        public Checkbox(ReactiveProperty<bool> value) : base(value) { }
    }
}
