using R3;
using UnityEngine.UIElements;

namespace ELEMENTS.Elements
{
    public class HorizontalGroup<T> : Group<T> where T : HorizontalGroup<T>
    {
        public HorizontalGroup(params IElement[] children)
            : base(children)
        {
            VisualElement.style.flexDirection = FlexDirection.Row;
        }

        public T Reverse(bool reverse = true)
        {
            VisualElement.style.flexDirection = reverse ? FlexDirection.RowReverse : FlexDirection.Row;
            return (T)this;
        }

        public bool GetReverse()
        {
            return VisualElement.style.flexDirection == FlexDirection.RowReverse;
        }

        public T BindReverse(Observable<bool> reverse)
        {
            Disposables.Add(reverse.Subscribe(nv => Reverse(nv)));
            return (T)this;
        }
    }

    public class HorizontalGroup : HorizontalGroup<HorizontalGroup>
    {
        public HorizontalGroup(params IElement[] children) : base(children) { }
    }
}