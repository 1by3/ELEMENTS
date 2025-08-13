using R3;

namespace ELEMENTS.Scripts.Elements
{
    public class TabItem<T> : Button<T> where T : TabItem<T>
    {
        public TabItem(params IElement[] children) : base(children)
        {
            VisualElement.AddToClassList("tab-item");
        }

        public T Active(bool active = true)
        {
            if (active) VisualElement.AddToClassList("active");
            else VisualElement.RemoveFromClassList("active");
            return (T)this;
        }

        public bool GetActive()
        {
            return VisualElement.ClassListContains("active");
        }

        public T BindActive(Observable<bool> active)
        {
            Disposables.Add(active.Subscribe(v => Active(v)));
            return (T)this;
        }

        public T First(bool first = true)
        {
            if (!first) VisualElement.RemoveFromClassList("first");
            else VisualElement.AddToClassList("first");
            return (T)this;
        }

        public bool GetFirst()
        {
            return VisualElement.ClassListContains("first");
        }

        public T BindFirst(Observable<bool> first)
        {
            Disposables.Add(first.Subscribe(v => First(v)));
            return (T)this;
        }

        public T Last(bool last = true)
        {
            if (!last) VisualElement.RemoveFromClassList("last");
            else VisualElement.AddToClassList("last");
            return (T)this;
        }

        public bool GetLast()
        {
            return VisualElement.ClassListContains("last");
        }

        public T BindLast(Observable<bool> last)
        {
            Disposables.Add(last.Subscribe(v => First(v)));
            return (T)this;
        }
    }

    public class TabItem : TabItem<TabItem>
    {
        public TabItem(params IElement[] children) : base(children)
        {
        }
    }
}